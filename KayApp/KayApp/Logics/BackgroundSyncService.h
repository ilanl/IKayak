//
//  SyncAdapter.h
//  KayApp
//
//  Created by Ilan Levy on 9/23/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Global.h"
#import "ASIHTTPRequest.h"
#import "PreferenceRequest.h"
#import "BookingRequest.h"
#import "DbAdapter.h"
#import "Reminder.h"

@interface BackgroundSyncService : NSObject

+ (BackgroundSyncService *)getInstance;

- (void)run;

- (void)reloadBookings;


@end