//
//  Vibration.mm
//  https://videogamecreation.fr
//
//  Created by Benoît Freslon on 23/03/2017.
//  Copyright © 2018 Benoît Freslon. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <AudioToolbox/AudioToolbox.h>
#import <UIKit/UIKit.h>
#import <CoreHaptics/CHHapticEngine.h>
#import <CoreHaptics/CHHapticPattern.h>
#import <CoreHaptics/CHHapticDeviceCapability.h>
#import <CoreHaptics/CHHapticEvent.h>

#import "Vibration.h"

#define USING_IPAD UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad

@interface Vibration ()

@end

@implementation Vibration

API_AVAILABLE(ios(13.0))
static CHHapticEngine * _engine;

//////////////////////////////////////////

#pragma mark - Vibrate

+ (BOOL)    hasVibrator {
    return !(USING_IPAD);
}
+ (void)    vibrate {
    AudioServicesPlaySystemSoundWithCompletion(1352, NULL);
}
+ (void)    vibratePeek {
    AudioServicesPlaySystemSoundWithCompletion(1519, NULL); // Actuate `Peek` feedback (weak boom)
}
+ (void)    vibratePop {
    AudioServicesPlaySystemSoundWithCompletion(1520, NULL); // Actuate `Pop` feedback (strong boom)
}
+ (void)    vibrateNope {
    AudioServicesPlaySystemSoundWithCompletion(1521, NULL); // Actuate `Nope` feedback (series of three weak booms)
}
+ (void)    init {
    NSError* error = nil;
    _engine = [[CHHapticEngine alloc] initAndReturnError:&error];
    [_engine startAndReturnError:&error];
}

+ (void)    vibrateWithParams:(long)time
{
    NSError* error = nil;
    CHHapticEventParameter *intensity = [[CHHapticEventParameter alloc] initWithParameterID:CHHapticEventParameterIDHapticIntensity value:0.7];
    
    CHHapticEventParameter *sharpness = [[CHHapticEventParameter alloc] initWithParameterID:CHHapticEventParameterIDHapticSharpness value:0.25];
    
    CHHapticEvent *event = [[CHHapticEvent alloc] initWithEventType:CHHapticEventTypeHapticContinuous parameters:[NSArray arrayWithObjects:intensity, sharpness, nil] relativeTime:0 duration:0.001 * time];
    CHHapticPattern *pattern = [[CHHapticPattern alloc] initWithEvents:[NSArray arrayWithObject:event] parameters:[[NSArray alloc] init] error:&error];
    
    id<CHHapticPatternPlayer> player = [_engine createPlayerWithPattern:pattern error:&error];
    [player startAtTime:0 error:&error];
}

+ (bool) isHapticSupported {
    id<CHHapticDeviceCapability> capabilities = [CHHapticEngine capabilitiesForHardware];
    return capabilities.supportsHaptics;
}

@end
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#pragma mark - "C"

extern "C" {
    
    //////////////////////////////////////////
    // Vibrate
    
    bool    _HasVibrator () {
        return [Vibration hasVibrator];
    }
 
    void    _Vibrate () {
        [Vibration vibrate];
    }
    
    void    _VibratePeek () {
        [Vibration vibratePeek];
    }
    void    _VibratePop () {
        [Vibration vibratePop];
    }
    void    _VibrateNope () {
        [Vibration vibrateNope];
    }
    void    _VibrateInit() {
        [Vibration init];
    }
    void _VibrateWithParam (long time)
    {
        [Vibration vibrateWithParams:time];
    }
    bool _IsHapticSupported () {
        return [Vibration isHapticSupported];
    }
}

