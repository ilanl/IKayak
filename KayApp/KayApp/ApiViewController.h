//
//  Api.h
//  KayApp
//
//  Created by Ilan Levy on 10/12/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AppDelegate.h"
#import "sqlite3.h"
#import "ASIHTTPRequestDelegate.h"
#import "PreferenceRequest.h"
#import "PreferenceResponse.h"
#import "ASIHTTPRequest.h"
#import "LightKayakPref.h"
#import "LightTimePref.h"
#import "BookingRequest.h"
#import "BookingResponse.h"
#import "Booking.h"
#import "Global.h"
#import "DbAdapter.h"
#import "Weather.h"

@interface ApiViewController : UIViewController<ASIHTTPRequestDelegate>

-(void) downloadAll:(NSString *)userName withPassword:(NSString *) password withActivityIndicator:(UIActivityIndicatorView *) activityView withController:(UIViewController *) controller;
@end
