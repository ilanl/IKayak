//
//  DRCAppDelegate.h
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Global.h"
#import "BackgroundSyncService.h"
#import "User.h"


@interface AppDelegate : UIResponder <UIApplicationDelegate>{
    // Instance member of our background task process
    UIBackgroundTaskIdentifier bgTask;
    
}

@property (strong, nonatomic) UIWindow *window;
@property (strong, nonatomic) NSMutableArray *tempCounterData;

extern AppDelegate *appContext;

@end
