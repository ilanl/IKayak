//
//  DRCAccountViewController.h
//  KayApp
//
//  Created by Ilan Levy on 9/20/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
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
#import "ApiViewController.h"

@interface LoginViewController : ApiViewController{

    sqlite3 *database;
    
}
@property (weak, nonatomic) IBOutlet UITextField *txtUser;
@property (weak, nonatomic) IBOutlet UITextField *txtPassword;

- (IBAction)LoginClick:(id)sender;

@end
