#define STOP_EXTRACTION_WHEN_WINDOW_CLOSED

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class UnitypackageExtractor : EditorWindow
{
	#region Helper Classes
	// Allows fetching the progress of GZipStream's decompression
	// Credit: https://stackoverflow.com/a/413352/2373034
	private class ProgressedFileStream : FileStream
	{
		private readonly UnitypackageExtractor progressTracker;

		public ProgressedFileStream( UnitypackageExtractor progressTracker, string path ) : base( path, FileMode.Open, FileAccess.Read )
		{
			this.progressTracker = progressTracker;
		}

		public override int Read( byte[] buffer, int offset, int count )
		{
			int bytesRead = base.Read( buffer, offset, count );
			progressTracker.decompressionProgressCurrent += bytesRead;
			progressTracker.needsRepaint = true;
			return bytesRead;
		}
	}

	// Displays contents of a unitypackage in a tree view
	private class AssetTreeView : TreeView
	{
		private class AssetData
		{
			public AssetTreeElement asset;
			public string label; // Label will include asset's filesize, as well

			public AssetData( AssetTreeElement asset, string label )
			{
				this.asset = asset;
				this.label = label;
			}
		}

		private readonly AssetTreeElement[] assets;
		private readonly Dictionary<int, AssetData> idToAssetLookup = new Dictionary<int, AssetData>( 512 );

		private readonly string trimmedDirectoryPath;

		private readonly GUIContent sharedGUIContent = new GUIContent();

		private string m_totalExtractFileSizeLabel;
		public string TotalExtractFileSizeLabel { get { return m_totalExtractFileSizeLabel; } }

		protected override bool CanMultiSelect( TreeViewItem item ) { return false; }
		protected override bool CanRename( TreeViewItem item ) { return false; }
		protected override void RenameEnded( RenameEndedArgs args ) { }

		public AssetTreeView( TreeViewState treeViewState, AssetTreeElement[] assets, string trimmedDirectoryPath ) : base( treeViewState )
		{
			this.assets = assets;
			this.trimmedDirectoryPath = trimmedDirectoryPath != null ? ( trimmedDirectoryPath + "/" ) : null;

			showBorder = true;
#if UNITY_2019_3_OR_NEWER
			depthIndentWidth += 5f;
#endif

			// Draw only the visible rows. This requires setting useScrollView to false because we are using an external scroll view: https://docs.unity3d.com/ScriptReference/IMGUI.Controls.TreeView-useScrollView.html
#if UNITY_2018_2_OR_NEWER
			useScrollView = false;
#else
			object treeViewController = typeof( TreeView ).GetField( "m_TreeView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ).GetValue( this );
			treeViewController.GetType().GetMethod( "SetUseScrollView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ).Invoke( treeViewController, new object[1] { false } );
#endif

			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			Dictionary<string, AssetTreeElement> folderAssets = new Dictionary<string, AssetTreeElement>( 64 );
			Dictionary<string, TreeViewItem> directories = new Dictionary<string, TreeViewItem>( 64 );

			TreeViewItem root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			int id = 1;

			for( int i = 0; i < assets.Length; i++ )
			{
				AssetTreeElement asset = assets[i];
				if( asset.isFolder )
					folderAssets[asset.path] = asset;
			}

			for( int i = 0; i < assets.Length; i++ )
			{
				AssetTreeElement asset = assets[i];
				if( asset.isFolder )
					continue;

				// Create TreeViewItem for asset
				idToAssetLookup[id] = new AssetData( asset, string.Concat( asset.filename, "  (", EditorUtility.FormatBytes( asset.fileSize ), ")" ) );
				TreeViewItem assetTreeItem = new TreeViewItem( id++, 0, asset.filename );

				// Create TreeViewItem for asset's parent directory
				string directoryPath = Path.GetDirectoryName( asset.path ).Replace( '\\', '/' );
				TreeViewItem prevDirectory = null;
				bool directoryRegisteredBefore = false;
				while( !string.IsNullOrEmpty( directoryPath ) )
				{
					TreeViewItem directory;
					if( directories.TryGetValue( directoryPath, out directory ) )
						directoryRegisteredBefore = true;
					else
					{
						AssetTreeElement folderAsset;
						if( !folderAssets.TryGetValue( directoryPath, out folderAsset ) )
						{
							// unitypackage doesn't have a meta file for this folder, create a dummy folder holder
							folderAsset = new AssetTreeElement( directoryPath, null ) { isFolder = true };
							folderAssets[directoryPath] = folderAsset;
						}

						idToAssetLookup[id] = new AssetData( folderAsset, "" ); // Folder labels will be calculated in CalculateTotalExtractFileSizeDownwards

						directory = new TreeViewItem( id++, 0, folderAsset.filename );
						directories[directoryPath] = directory;
					}

					// Don't include trimmed directories in the TreeView
					bool validDirectory = trimmedDirectoryPath == null || !StartsWithFast( trimmedDirectoryPath, directoryPath + "/" );
					if( validDirectory )
					{
						if( prevDirectory != null )
							directory.AddChild( prevDirectory );
						else
							directory.AddChild( assetTreeItem );

						prevDirectory = directory;
					}

					if( directoryRegisteredBefore )
					{
						// If this directory is trimmed (i.e. validDirectory=false), set directoryRegisteredBefore to
						// false so that "else if( !directoryRegisteredBefore )" condition below is executed and
						// no valid directories are excluded from the TreeView
						directoryRegisteredBefore = directoryRegisteredBefore && validDirectory;
						break;
					}

					directoryPath = Path.GetDirectoryName( directoryPath ).Replace( '\\', '/' );
				}

				if( prevDirectory == null )
					root.AddChild( assetTreeItem );
				else if( !directoryRegisteredBefore )
					root.AddChild( prevDirectory );
			}

			// Refresh folders' extract properties
			if( root.hasChildren )
			{
				List<TreeViewItem> rootContents = root.children;
				for( int i = 0; i < rootContents.Count; i++ )
					RefreshExtractDownwards( rootContents[i] );
			}

			RecalculateTotalExtractFileSize( root );

			SetupDepthsFromParentsAndChildren( root );
			return root;
		}

		public HashSet<string> GetAllowedAssetGUIDs()
		{
			HashSet<string> result = new HashSet<string>();

			bool everythingExtracted = true;
			for( int i = 0; i < assets.Length; i++ )
			{
				if( assets[i].extract == AssetTreeElement.Extract.No )
					everythingExtracted = false;
				else if( !string.IsNullOrEmpty( assets[i].guid ) )
					result.Add( assets[i].guid );
			}

			return everythingExtracted ? null : result; // We don't use HashSet when everything is extracted to improve performance
		}

		protected override void RowGUI( RowGUIArgs args )
		{
			AssetData assetData = idToAssetLookup[args.item.id];
			AssetTreeElement asset = assetData.asset;

			sharedGUIContent.text = assetData.label;
			sharedGUIContent.image = asset.isFolder ? AssetDatabase.GetCachedIcon( "Assets" ) as Texture2D : UnityEditorInternal.InternalEditorUtility.GetIconForFile( asset.path );

			Rect rect = args.rowRect;
			rect.xMin += GetContentIndent( args.item );

			if( asset.isFolder && asset.extract == AssetTreeElement.Extract.Mixed )
				EditorGUI.showMixedValue = true;

			EditorGUI.BeginChangeCheck();
			bool shouldExtract = EditorGUI.Toggle( new Rect( rect.x, rect.y, rect.height, rect.height ), asset.extract == AssetTreeElement.Extract.Yes );
			if( EditorGUI.EndChangeCheck() )
			{
				asset.extract = shouldExtract ? AssetTreeElement.Extract.Yes : AssetTreeElement.Extract.No;

				if( asset.isFolder )
					SetExtractDownwards( args.item, shouldExtract );

				if( args.item.parent.id > 0 )
					RefreshExtractUpwards( args.item.parent );

				RecalculateTotalExtractFileSize( rootItem );
			}

			EditorGUI.showMixedValue = false;

			rect.xMin += rect.height;
			rect.y -= 2f;
			rect.height += 4f; // Incrementing height fixes cropped icon issue on Unity 2019.2 or earlier

			GUI.Label( rect, sharedGUIContent );
		}

		public void SetExtractAll( bool extract )
		{
			SetExtractDownwards( rootItem, extract );
			RecalculateTotalExtractFileSize( rootItem );
		}

		private void SetExtractDownwards( TreeViewItem folder, bool extract )
		{
			if( folder.hasChildren )
			{
				List<TreeViewItem> folderContents = folder.children;
				for( int i = 0; i < folderContents.Count; i++ )
				{
					AssetTreeElement folderContent = idToAssetLookup[folderContents[i].id].asset;
					folderContent.extract = extract ? AssetTreeElement.Extract.Yes : AssetTreeElement.Extract.No;
					if( folderContent.isFolder )
						SetExtractDownwards( folderContents[i], extract );
				}
			}
		}

		private void RefreshExtractUpwards( TreeViewItem folder )
		{
			AssetTreeElement _folder = idToAssetLookup[folder.id].asset;
			List<TreeViewItem> folderContents = folder.children;

			AssetTreeElement.Extract extractMode = AssetTreeElement.Extract.Yes;
			for( int i = 0; i < folderContents.Count; i++ )
			{
				AssetTreeElement folderContent = idToAssetLookup[folderContents[i].id].asset;
				if( i == 0 )
					extractMode = folderContent.extract;
				else if( folderContent.extract != extractMode )
				{
					extractMode = AssetTreeElement.Extract.Mixed;
					break;
				}
			}

			_folder.extract = extractMode;

			if( folder.parent.id > 0 )
				RefreshExtractUpwards( folder.parent );
		}

		// Returns the total size of files in the folder
		private long RefreshExtractDownwards( TreeViewItem folder )
		{
			if( !folder.hasChildren )
				return 0L;

			AssetTreeElement _folder = idToAssetLookup[folder.id].asset;
			List<TreeViewItem> folderContents = folder.children;

			// Calculate total size of files in the folder
			_folder.fileSize = 0L;

			AssetTreeElement.Extract extractMode = AssetTreeElement.Extract.Yes;
			for( int i = 0; i < folderContents.Count; i++ )
			{
				AssetTreeElement folderContent = idToAssetLookup[folderContents[i].id].asset;
				if( folderContent.isFolder )
					_folder.fileSize += RefreshExtractDownwards( folderContents[i] );
				else
					_folder.fileSize += folderContent.fileSize;

				if( i == 0 )
					extractMode = folderContent.extract;
				else if( folderContent.extract != extractMode )
					extractMode = AssetTreeElement.Extract.Mixed;
			}

			_folder.extract = extractMode;
			return _folder.fileSize;
		}

		// Calculates total size of files that will be extracted
		private void RecalculateTotalExtractFileSize( TreeViewItem root )
		{
			long totalExtractFileSize = 0L, totalSize = 0L;
			if( root.hasChildren )
			{
				List<TreeViewItem> rootContents = root.children;
				for( int i = 0; i < rootContents.Count; i++ )
				{
					totalExtractFileSize += CalculateTotalExtractFileSizeDownwards( rootContents[i] );
					totalSize += idToAssetLookup[rootContents[i].id].asset.fileSize;
				}
			}

			m_totalExtractFileSizeLabel = string.Concat( "Total size: ", EditorUtility.FormatBytes( totalExtractFileSize ), " / ", EditorUtility.FormatBytes( totalSize ) );
		}

		// Returns the total size of extracted files in the folder
		private long CalculateTotalExtractFileSizeDownwards( TreeViewItem folder )
		{
			AssetData folderData = idToAssetLookup[folder.id];
			AssetTreeElement _folder = folderData.asset;

			long totalExtractFileSize = 0L;
			if( folder.hasChildren )
			{
				List<TreeViewItem> folderContents = folder.children;
				for( int i = 0; i < folderContents.Count; i++ )
				{
					AssetTreeElement folderContent = idToAssetLookup[folderContents[i].id].asset;
					if( folderContent.isFolder )
						totalExtractFileSize += CalculateTotalExtractFileSizeDownwards( folderContents[i] );
					else if( folderContent.extract == AssetTreeElement.Extract.Yes )
						totalExtractFileSize += folderContent.fileSize;
				}
			}

			// Folder labels should look like this: "SomeFolder (1.2 KB / 2.0 MB)"
			folderData.label = string.Concat( _folder.filename, "  (", EditorUtility.FormatBytes( totalExtractFileSize ), " / ", EditorUtility.FormatBytes( _folder.fileSize ), ")" );

			return totalExtractFileSize;
		}
	}

	[Serializable]
	private class AssetTreeElement
	{
		public enum Extract { Yes = 0, No = 1, Mixed = 2 };

		public string path;
		public string guid;
		public string filename;
		public long fileSize;
		public Extract extract = Extract.Yes;
		public bool isFolder = false;

		public AssetTreeElement( string path, string guid )
		{
			this.path = path;
			this.guid = guid;
			this.filename = Path.GetFileName( path );
		}
	}
	#endregion

	private const int BUFFER_SIZE = 4096; // Must be at least 512
	private const string TRIMMED_DIRECTORY_NONE = "NONE";

	private string packagePath, outputPath;

	private bool isWorking, isDecompressing, isCalculatingAssetTree, needsRepaint;
	private long decompressionProgressCurrent, decompressionProgressTotal;
	private int renameProgressCurrent, renameProgressTotal;

	private string[] trimmableDirectories;
	private int trimmedDirectoryIndex = -1;
	private string trimmedDirectoryPath = TRIMMED_DIRECTORY_NONE;

	private bool assetDatabaseLocked;
	private bool assemblyReloadLockedHint;

	private Thread runningThread;
	private bool abortThread;

	private AssetTreeElement[] assetTree;
	private AssetTreeView assetTreeView;
	private TreeViewState assetTreeViewState;
	private SearchField searchField;

	private Vector2 scrollPos;

	private readonly GUIContent pickFolderButtonContent = new GUIContent( "o", "Click to pick a folder. Right click to open Asset Store cache" );
	private readonly GUIContent generateAssetTreeButtonContent = new GUIContent( "Select Assets to Extract...", "Select which assets to export from the unitypackage" );

	[MenuItem( "Window/Unitypackage Extractor" )]
	private static void Init()
	{
		UnitypackageExtractor window = GetWindow<UnitypackageExtractor>();
		window.titleContent = new GUIContent( "Extractor" );
		window.minSize = new Vector2( 300f, 100f );
		window.Show();
	}

	private void OnEnable()
	{
		GenerateAssetTreeView();
	}

#if STOP_EXTRACTION_WHEN_WINDOW_CLOSED
	private void OnDestroy()
	{
		abortThread = true;
	}
#endif

	private void Update()
	{
		if( needsRepaint )
		{
			needsRepaint = false;
			Repaint();
		}
	}

	private void OnGUI()
	{
		scrollPos = GUILayout.BeginScrollView( scrollPos );

		GUI.enabled = !isWorking;

		float labelWidth = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 120f;

		string newPackagePath = PathField( ".unitypackage Path: ", packagePath, false );
		if( newPackagePath != packagePath )
		{
			packagePath = newPackagePath;

			trimmableDirectories = null;
			trimmedDirectoryIndex = -1;
			trimmedDirectoryPath = TRIMMED_DIRECTORY_NONE;

			assetTree = null;
			assetTreeView = null;
			assetTreeViewState = null;
			searchField = null;
		}

		outputPath = PathField( "Output Path: ", outputPath, true );

		EditorGUIUtility.labelWidth = labelWidth;

		if( assetTreeView != null )
		{
			if( trimmableDirectories != null && trimmableDirectories.Length > 0 )
			{
				GUILayout.BeginVertical( GUI.skin.box );

				GUILayout.Label( "Trim Prefix: " + trimmedDirectoryPath, EditorStyles.boldLabel );

				GUILayout.BeginHorizontal();

				if( GUILayout.Toggle( trimmedDirectoryIndex == -1, "NONE", EditorStyles.toolbarButton ) && trimmedDirectoryIndex != -1 )
				{
					trimmedDirectoryIndex = -1;
					trimmedDirectoryPath = TRIMMED_DIRECTORY_NONE;

					GenerateAssetTreeView();
				}

				for( int i = 0; i < trimmableDirectories.Length; i++ )
				{
					if( GUILayout.Toggle( trimmedDirectoryIndex == i, trimmableDirectories[i], EditorStyles.toolbarButton ) && trimmedDirectoryIndex != i )
					{
						trimmedDirectoryIndex = i;
						trimmedDirectoryPath = trimmableDirectories[0];
						for( int j = 1; j <= i; j++ )
							trimmedDirectoryPath += "/" + trimmableDirectories[j];

						GenerateAssetTreeView();
					}
				}

				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}

			GUILayout.BeginHorizontal();
			GUILayout.Label( "All:" );
			if( GUILayout.Button( "Select" ) )
				assetTreeView.SetExtractAll( true );
			if( GUILayout.Button( "Deselect" ) )
				assetTreeView.SetExtractAll( false );
			if( GUILayout.Button( "Expand" ) )
				assetTreeView.ExpandAll();
			if( GUILayout.Button( "Collapse" ) )
				assetTreeView.CollapseAll();
			GUILayout.EndHorizontal();

			GUILayout.Space( 3f );

			assetTreeView.searchString = searchField.OnGUI( assetTreeView.searchString );
			assetTreeView.OnGUI( EditorGUILayout.GetControlRect( false, assetTreeView.totalHeight ) );

			GUILayout.Label( assetTreeView.TotalExtractFileSizeLabel );
			EditorGUILayout.Space();
		}

		GUI.enabled = true;

		if( !isWorking )
		{
			if( ( assetTree == null || assetTree.Length == 0 ) && GUILayout.Button( generateAssetTreeButtonContent ) )
			{
				packagePath = packagePath.Trim();

				if( string.IsNullOrEmpty( packagePath ) || !File.Exists( packagePath ) )
				{
					Debug.LogError( ".unitypackage doesn't exist at: " + packagePath );
					return;
				}

				ExecuteOnSeparateThread( true, false );
			}

			if( GUILayout.Button( "Extract!" ) )
			{
				packagePath = packagePath.Trim();
				outputPath = outputPath.Trim();
				if( outputPath.Length > 0 && ( outputPath[outputPath.Length - 1] == '/' || outputPath[outputPath.Length - 1] == '\\' ) )
					outputPath = outputPath.Substring( 0, outputPath.Length - 1 );

				if( string.IsNullOrEmpty( packagePath ) || !File.Exists( packagePath ) )
				{
					Debug.LogError( ".unitypackage doesn't exist at: " + packagePath );
					return;
				}

				if( string.IsNullOrEmpty( outputPath ) )
				{
					Debug.LogError( "Output Path can't be blank" );
					return;
				}

				if( !Directory.Exists( outputPath ) )
					Directory.CreateDirectory( outputPath );
				else if( Directory.GetFileSystemEntries( outputPath ).Length > 0 && !EditorUtility.DisplayDialog( "Warning", "Output Path isn't an empty folder. Conflicting files will be overridden. Proceed?", "OK", "Cancel" ) )
					return;

				// If we are extracting assets to this Unity project, lock AssetDatabase for safety reasons
				// Assets are considered to be extracted to this Unity project if:
				// a) outputPath points to the root of the Unity project directory
				// b) outputPath points to either Assets folder or a subfolder of it
				// In other words, if we extract the assets to PROJECT_PATH/Library or PROJECT_PATH/SomeCustomFolder, we don't have to lock AssetDatabase
				string projectAbsolutePath = Path.GetFullPath( Directory.GetCurrentDirectory() ) + "/";
				string outputAbsolutePath = Path.GetFullPath( outputPath ) + "/";
				bool lockAssetDatabase = projectAbsolutePath == outputAbsolutePath || StartsWithFast( outputAbsolutePath, Path.GetFullPath( "Assets" ) );
				if( lockAssetDatabase && !EditorUtility.DisplayDialog( "Warning", "Unitypackage will be extracted to this Unity project. To avoid any potential issues, you shouldn't modify your scenes, assets or scripts until the extraction is completed. Proceed?", "OK", "Cancel" ) )
					return;

				ExecuteOnSeparateThread( false, lockAssetDatabase );
			}
		}
		else if( GUILayout.Button( "Stop" ) )
			abortThread = true;

		if( isWorking )
		{
			Rect progressbarRect = EditorGUILayout.GetControlRect( false, EditorGUIUtility.singleLineHeight );

			if( isDecompressing )
			{
				if( decompressionProgressTotal > 0L )
					EditorGUI.ProgressBar( progressbarRect, (float) ( (double) decompressionProgressCurrent / decompressionProgressTotal ), isCalculatingAssetTree ? "Generating Asset Tree" : "Decompressing Archive" );
			}
			else if( renameProgressTotal > 0 )
				EditorGUI.ProgressBar( progressbarRect, (float) renameProgressCurrent / renameProgressTotal, string.Concat( "Renaming Assets: ", renameProgressCurrent, "/", renameProgressTotal ) );
		}

		GUILayout.Space( 5f );

		GUILayout.EndScrollView();
	}

	private void GenerateAssetTreeView()
	{
		if( assetTree != null && assetTree.Length > 0 )
		{
			bool shouldExpandAll = false;
			if( assetTreeViewState == null )
			{
				assetTreeViewState = new TreeViewState();
				shouldExpandAll = true;
			}

			assetTreeView = new AssetTreeView( assetTreeViewState, assetTree, trimmedDirectoryIndex >= 0 ? trimmedDirectoryPath : null );
			searchField = new SearchField();
			searchField.downOrUpArrowKeyPressed += assetTreeView.SetFocusAndEnsureSelectedItem;

			if( shouldExpandAll )
				assetTreeView.ExpandAll();
		}
	}

	private string PathField( string label, string path, bool isDirectory )
	{
		GUILayout.BeginHorizontal();

		path = EditorGUILayout.TextField( label, path );

		if( GUILayout.Button( pickFolderButtonContent, GUILayout.Width( 25f ) ) )
		{
			string initialPath = "";
			try
			{
				if( Event.current.button == 1 )
				{
					initialPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
					if( !string.IsNullOrEmpty( initialPath ) )
						initialPath = Path.Combine( initialPath, "Unity/Asset Store-5.x" );

					if( !Directory.Exists( initialPath ) )
						initialPath = "";
				}

				if( string.IsNullOrEmpty( initialPath ) && !string.IsNullOrEmpty( path ) )
				{
					if( Directory.Exists( path ) )
						initialPath = path;
					else
					{
						string parentPath = Path.GetDirectoryName( path );
						if( !string.IsNullOrEmpty( parentPath ) && Directory.Exists( parentPath ) )
							initialPath = parentPath;
					}
				}
			}
			catch
			{
				initialPath = "";
			}

			string selectedPath;
			if( isDirectory )
				selectedPath = EditorUtility.OpenFolderPanel( "Choose output directory", initialPath, "" );
			else
				selectedPath = EditorUtility.OpenFilePanel( "Choose .unitypackage", initialPath, "unitypackage" );

			if( !string.IsNullOrEmpty( selectedPath ) )
				path = selectedPath;

			GUIUtility.keyboardControl = 0; // Remove focus from active text field
		}

		GUILayout.EndHorizontal();

		return path;
	}

	private void ExecuteOnSeparateThread( bool isCalculatingAssetTree, bool lockAssetDatabase )
	{
		abortThread = false;
		isDecompressing = true;
		this.isCalculatingAssetTree = isCalculatingAssetTree;

		if( lockAssetDatabase )
		{
			AssetDatabase.StartAssetEditing();
			assetDatabaseLocked = true;
		}
		else
			assetDatabaseLocked = false;

		try
		{
			runningThread = isCalculatingAssetTree ? new Thread( GenerateAssetTree ) : new Thread( ExtractAssets );
			runningThread.Start();

			scrollPos.y = 99999f; // Scroll to the bottom to see the progressbar

			assemblyReloadLockedHint = false;
			isWorking = true;

			EditorApplication.LockReloadAssemblies();
			EditorApplication.update -= UnlockAssembliesAfterExtraction;
			EditorApplication.update += UnlockAssembliesAfterExtraction;
		}
		catch
		{
			if( assetDatabaseLocked )
			{
				AssetDatabase.StopAssetEditing();
				assetDatabaseLocked = false;
			}

			throw;
		}
	}

	private void UnlockAssembliesAfterExtraction()
	{
		if( !isWorking )
		{
			try
			{
				EditorApplication.update -= UnlockAssembliesAfterExtraction;
				EditorApplication.UnlockReloadAssemblies();

				if( isCalculatingAssetTree )
					GenerateAssetTreeView();
			}
			finally
			{
				if( assetDatabaseLocked )
				{
					AssetDatabase.StopAssetEditing();
					AssetDatabase.Refresh();

					assetDatabaseLocked = false;
				}
			}
		}
		else
		{
			if( EditorApplication.isPlayingOrWillChangePlaymode )
			{
				EditorApplication.isPlaying = false;
				Debug.LogWarning( "Can't enter Play mode while extracting a Unitypackage, <b>Stop</b> the operation first!" );
			}

			if( !assemblyReloadLockedHint && EditorApplication.isCompiling )
			{
				assemblyReloadLockedHint = true;
				Debug.LogWarning( "Can't reload assemblies while extracting a Unitypackage, <b>Stop</b> the operation first!" );
			}
		}
	}

	private void GenerateAssetTree()
	{
		try
		{
			AssetTreeElement[] assetTree;

			string packagePath = this.packagePath;

			decompressionProgressCurrent = 0L;
			decompressionProgressTotal = 0L;

			using( ProgressedFileStream fs = new ProgressedFileStream( this, packagePath ) )
			using( GZipStream gzip = new GZipStream( fs, CompressionMode.Decompress ) )
			{
				decompressionProgressTotal = fs.Length;
				assetTree = GenerateAssetTreeFromTarGz( gzip );
				isDecompressing = false;
			}

			assetTreeView = null;
			searchField = null;

			this.assetTree = assetTree;

			if( abortThread )
				Debug.Log( "<b>Stopped generating asset tree:</b> " + packagePath );
			else if( assetTree != null && assetTree.Length > 0 )
			{
				// Calculate trimmableDirectories
				string sharedParentDirectories = null;
				for( int i = 0; i < assetTree.Length; i++ )
				{
					AssetTreeElement asset = assetTree[i];
					if( asset.isFolder )
						continue;

					string directoryPath = Path.GetDirectoryName( asset.path ).Replace( '\\', '/' );

					// Find common parent directories of the assets in order to populate trimmableDirectories
					if( sharedParentDirectories == null )
						sharedParentDirectories = directoryPath;
					else if( sharedParentDirectories.Length > 0 )
					{
						int minLength = Mathf.Min( sharedParentDirectories.Length, directoryPath.Length );
						int lastPathSeparatorIndex = 0, iterator = 0;
						for( ; iterator < minLength; iterator++ )
						{
							char ch = sharedParentDirectories[iterator];
							if( ch != directoryPath[iterator] )
								break;

							if( ch == '/' )
								lastPathSeparatorIndex = iterator;
						}

						if( iterator == minLength )
							lastPathSeparatorIndex = iterator;

						sharedParentDirectories = sharedParentDirectories.Substring( 0, lastPathSeparatorIndex );
					}
				}

				trimmableDirectories = sharedParentDirectories.Split( '/' );
			}
		}
		catch( Exception e )
		{
			Debug.LogException( e );
		}

		isWorking = false;
		needsRepaint = true;
	}

	private void ExtractAssets()
	{
		try
		{
			string[] extractedDirectories;

			string packagePath = this.packagePath;
			string outputPath = this.outputPath;
			string trimmedDirectoryPathWithSeparator = trimmedDirectoryIndex >= 0 ? ( trimmedDirectoryPath + "/" ) : null;

			decompressionProgressCurrent = 0L;
			decompressionProgressTotal = 0L;
			renameProgressCurrent = 0;
			renameProgressTotal = 0;

			using( ProgressedFileStream fs = new ProgressedFileStream( this, packagePath ) )
			using( GZipStream gzip = new GZipStream( fs, CompressionMode.Decompress ) )
			{
				decompressionProgressTotal = fs.Length;
				extractedDirectories = ExtractAssetsFromTarGz( gzip, outputPath, assetTreeView != null ? assetTreeView.GetAllowedAssetGUIDs() : null );
				isDecompressing = false;
			}

			foreach( string directory in extractedDirectories )
			{
				if( abortThread )
					break;

				string pathnameFile = Path.Combine( directory, "pathname" );
				if( !File.Exists( pathnameFile ) )
					continue;

				string path = File.ReadAllText( pathnameFile );
				int newLineIndex = path.IndexOf( '\n' );
				if( newLineIndex > 0 )
					path = path.Substring( 0, newLineIndex );

				path = path.Trim();

				// Trim common prefix directories from path, if desired
				if( trimmedDirectoryPathWithSeparator != null )
				{
					if( path.Length < trimmedDirectoryPathWithSeparator.Length && StartsWithFast( trimmedDirectoryPathWithSeparator, path + "/" ) )
						continue;
					else if( StartsWithFast( path, trimmedDirectoryPathWithSeparator ) )
						path = path.Substring( trimmedDirectoryPathWithSeparator.Length );
				}

				path = Path.Combine( outputPath, path );
				Directory.CreateDirectory( Path.GetDirectoryName( path ) );

				string assetFile = Path.Combine( directory, "asset" );
				if( File.Exists( assetFile ) )
				{
					if( File.Exists( path ) )
						File.Copy( assetFile, path, true );
					else
						File.Move( assetFile, path );
				}
				else // This is a directory
					Directory.CreateDirectory( path );

				string assetMetaFile = Path.Combine( directory, "asset.meta" );
				if( File.Exists( assetMetaFile ) )
				{
					string metaPath = path + ".meta";
					if( File.Exists( metaPath ) )
						File.Copy( assetMetaFile, metaPath, true );
					else
						File.Move( assetMetaFile, metaPath );
				}

				renameProgressCurrent++;
				needsRepaint = true;
			}

			// Cleanup temporary directories
			foreach( string directory in extractedDirectories )
				Directory.Delete( directory, true );

			if( abortThread )
				Debug.Log( "<b>Stopped extracting:</b> " + packagePath );
			else
				Debug.Log( "<b>Finished extracting:</b> " + packagePath );
		}
		catch( Exception e )
		{
			Debug.LogException( e );
		}

		isWorking = false;
		needsRepaint = true;
	}

	// Credit: https://gist.github.com/davetransom/553aeb3c4388c3eb448c0afe564cd2e3
	private AssetTreeElement[] GenerateAssetTreeFromTarGz( GZipStream input )
	{
		List<AssetTreeElement> assetTree = new List<AssetTreeElement>( 512 );
		Dictionary<string, long> assetTreeSizes = new Dictionary<string, long>( 512 );

		byte[] buffer = new byte[BUFFER_SIZE];
		while( true )
		{
			if( abortThread )
				return new AssetTreeElement[0];

			input.Read( buffer, 0, 512 );

			string name = Encoding.ASCII.GetString( buffer, 0, 100 ).Trim( '\0' ).Trim();
			if( name.Length == 0 )
				break;
			else if( StartsWithFast( name, "./" ) )
			{
				if( name.Length == 2 )
					continue;

				name = name.Substring( 2 );
			}

			long size = Convert.ToInt64( Encoding.ASCII.GetString( buffer, 124, 12 ).Trim( '\0', ' ' ), 8 );
			if( size > 0 ) // This is a file entry
			{
				if( Path.GetFileName( name ) == "pathname" )
				{
					// Fetch asset's path
					using( MemoryStream memoryStream = new MemoryStream( 128 ) )
					{
						long remaining = size;
						int bytesRead;
						while( ( bytesRead = input.Read( buffer, 0, remaining >= BUFFER_SIZE ? BUFFER_SIZE : (int) remaining ) ) > 0 )
						{
							memoryStream.Write( buffer, 0, bytesRead );
							remaining -= bytesRead;
						}

						string assetPath = Encoding.UTF8.GetString( memoryStream.ToArray() );
						int newLineIndex = assetPath.IndexOf( '\n' );
						if( newLineIndex > 0 )
							assetPath = assetPath.Substring( 0, newLineIndex );

						assetTree.Add( new AssetTreeElement( assetPath, Path.GetDirectoryName( name ) ) );
					}
				}
				else
				{
					if( EndsWithFast( name, "/asset" ) )
						assetTreeSizes[Path.GetDirectoryName( name )] = size;

					// Skip the file contents
					long remaining = size;
					int bytesRead;
					while( ( bytesRead = input.Read( buffer, 0, remaining >= BUFFER_SIZE ? BUFFER_SIZE : (int) remaining ) ) > 0 )
						remaining -= bytesRead;
				}
			}

			int offset = 512 - (int) ( size % 512 );
			if( offset > 0 && offset < 512 )
				input.Read( buffer, 0, offset );
		}

		// Mark folders inside assetTree
		HashSet<string> directoriesInAssetTree = new HashSet<string>();

		for( int i = 0; i < assetTree.Count; i++ )
		{
			long assetSize;
			if( assetTreeSizes.TryGetValue( assetTree[i].guid, out assetSize ) )
				assetTree[i].fileSize = assetSize;

			string directoryPath = Path.GetDirectoryName( assetTree[i].path ).Replace( '\\', '/' );
			while( !string.IsNullOrEmpty( directoryPath ) )
			{
				if( directoriesInAssetTree.Contains( directoryPath ) )
					break;

				// Check if this directory's path is equal to an asset's path in assetTree
				for( int j = assetTree.Count - 1; j >= 0; j-- )
				{
					if( assetTree[j].path == directoryPath )
						assetTree[j].isFolder = true;
				}

				directoriesInAssetTree.Add( directoryPath );
				directoryPath = Path.GetDirectoryName( directoryPath ).Replace( '\\', '/' );
			}
		}

		assetTree.Sort( ( a1, a2 ) => a1.path.CompareTo( a2.path ) );

		return assetTree.ToArray();
	}

	// Credit: https://gist.github.com/davetransom/553aeb3c4388c3eb448c0afe564cd2e3
	private string[] ExtractAssetsFromTarGz( GZipStream input, string outputDir, HashSet<string> allowedAssetGUIDs )
	{
		List<string> extractedDirectories = new List<string>( 64 );

		byte[] buffer = new byte[BUFFER_SIZE];
		while( true )
		{
			if( abortThread )
				return new string[0];

			input.Read( buffer, 0, 512 );

			string name = Encoding.ASCII.GetString( buffer, 0, 100 ).Trim( '\0' ).Trim();
			if( name.Length == 0 )
				break;
			else if( StartsWithFast( name, "./" ) )
			{
				if( name.Length == 2 )
					continue;

				name = name.Substring( 2 );
			}

			long size = Convert.ToInt64( Encoding.ASCII.GetString( buffer, 124, 12 ).Trim( '\0', ' ' ), 8 );
			if( size > 0 ) // This is a file entry
			{
				// ".icon.png": no need to extract unitypackage's thumbnail
				if( ( allowedAssetGUIDs == null || allowedAssetGUIDs.Contains( Path.GetDirectoryName( name ) ) ) && name != ".icon.png" )
				{
					// Extract the file
					string output = Path.Combine( outputDir, name );
					Directory.CreateDirectory( Path.GetDirectoryName( output ) );

					using( FileStream fs = File.Open( output, FileMode.OpenOrCreate, FileAccess.Write ) )
					{
						long remaining = size;
						int bytesRead;
						while( ( bytesRead = input.Read( buffer, 0, remaining >= BUFFER_SIZE ? BUFFER_SIZE : (int) remaining ) ) > 0 )
						{
							fs.Write( buffer, 0, bytesRead );
							remaining -= bytesRead;
						}
					}
				}
				else
				{
					// Skip the file contents
					long remaining = size;
					int bytesRead;
					while( ( bytesRead = input.Read( buffer, 0, remaining >= BUFFER_SIZE ? BUFFER_SIZE : (int) remaining ) ) > 0 )
						remaining -= bytesRead;
				}
			}
			else // This is a directory entry
			{
				if( name.Length > 1 && name[name.Length - 1] == '/' )
					name = name.Substring( 0, name.Length - 1 );

				if( allowedAssetGUIDs == null || allowedAssetGUIDs.Contains( name ) )
				{
					// After extraction, contents of each directory will be renamed to match the target asset's name
					renameProgressTotal++;
					extractedDirectories.Add( Path.Combine( outputDir, name ) );
				}
			}

			int offset = 512 - (int) ( size % 512 );
			if( offset > 0 && offset < 512 )
				input.Read( buffer, 0, offset );
		}

		for( int i = extractedDirectories.Count - 1; i >= 0; i-- )
		{
			if( !Directory.Exists( extractedDirectories[i] ) )
				extractedDirectories.RemoveAt( i );
		}

		return extractedDirectories.ToArray();
	}

	// Returns true is str starts with prefix
	private static bool StartsWithFast( string str, string prefix )
	{
		int aLen = str.Length;
		int bLen = prefix.Length;
		int ap = 0; int bp = 0;
		while( ap < aLen && bp < bLen && str[ap] == prefix[bp] )
		{
			ap++;
			bp++;
		}

		return bp == bLen;
	}

	// Returns true is str ends with postfix
	private static bool EndsWithFast( string str, string postfix )
	{
		int aLen = str.Length - 1;
		int bLen = postfix.Length - 1;
		while( aLen >= 0 && bLen >= 0 && str[aLen] == postfix[bLen] )
		{
			aLen--;
			bLen--;
		}

		return bLen < 0;
	}
}