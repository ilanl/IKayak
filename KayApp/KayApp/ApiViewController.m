//
//  Api.m
//  KayApp
//
//  Created by Ilan Levy on 10/12/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "ApiViewController.h"

@implementation ApiViewController
{
    NSString* encodedUser;;
    NSString* encodedPassword;
    UIActivityIndicatorView* activityIndicator;
    UIViewController* sender;
}


-(void) downloadAll:(NSString *)userName withPassword:(NSString *) password withActivityIndicator:(UIActivityIndicatorView *) activityView withController:(UIViewController *) controller
{
    
    dispatch_group_t group = dispatch_group_create();
    
    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^ {
        
        [AppLog Log:@"Get prefs - start"];
        
        encodedUser = userName;
        encodedPassword = password;
        activityIndicator = activityView;
        sender = controller;
        
        PreferenceRequest *requestObj = [PreferenceRequest new];
        
        [requestObj setAction:@"0"];
        [requestObj setUserName:userName];
        [requestObj setPassword:password];
        [requestObj setDeviceToken: [[DbAdapter getInstance] getDeviceToken]];
        
        NSString *jsonString = [requestObj toJSONString];
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://breezback.com/IKayak/preferences.ashx"]];
        __weak __block ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL: url];
        
        [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
        [req addRequestHeader:@"Content-Type" value:@"application/json"];
        [req appendPostData:[jsonString  dataUsingEncoding:NSUTF8StringEncoding]];
        [req setTimeOutSeconds:60];
        // Handle request response
        [req setCompletionBlock:^{
            __strong ASIHTTPRequest *requestInBlock = req;
            
            NSError *error = [req error];
            if (!error)
            {
                NSString *responseString = [requestInBlock responseString];
                
                NSError* err;
                
                PreferenceResponse *responseObj = [[PreferenceResponse alloc] initWithString:responseString error:&err];
                if (!err)
                {
                    if ([[responseObj Status] isEqual: @"Success"])
                    {
                        
                        [[DbAdapter getInstance] saveUserToLocal:encodedUser withPassword:password];
                        
                        [[DbAdapter getInstance] savePreferencesToLocal:responseObj];
                        [[DbAdapter getInstance] getCurrentUser];
                        
                    }
                }
            }
            
            
        }];
        
        [req setFailedBlock:^{
            
            [AppLog Log:@"service error"];
            
        }];
        [req startSynchronous];
        
        [AppLog Log:@"Get prefs - end"];
        
    });
    
    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^ {
        
        [AppLog Log:@"Get forecasts - start"];
        [[Weather getInstance] reload];
        [AppLog Log:@"Get forecasts - end"];
        
    });

    dispatch_group_async(group,dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^ {
        
        [AppLog Log:@"Get bookings - start"];
        
        BookingRequest *requestObj = [BookingRequest new];
        
        [requestObj setAction:@"0"];
        [requestObj setUserName:encodedUser];
        [requestObj setPassword:encodedPassword];
        [requestObj setDeviceToken: [[DbAdapter getInstance] getDeviceToken]];
        
        
        NSString *jsonString = [requestObj toJSONString];
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://breezback.com/IKayak/bookings.ashx"]];
        __weak __block ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL: url];
        
        [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
        [req addRequestHeader:@"Content-Type" value:@"application/json"];
        [req appendPostData:[jsonString  dataUsingEncoding:NSUTF8StringEncoding]];
        [req setTimeOutSeconds:60];
        // Handle request response
        [req setCompletionBlock:^{
            
            __strong ASIHTTPRequest *requestInBlock = req;
            NSError *error = [req error];
            if (!error)
            {
                NSString *responseString = [requestInBlock responseString];
                
                NSError* err;
                
                BookingResponse *responseObj = [[BookingResponse alloc] initWithString:responseString error:&err];
                if (!err)
                {
                    if ([[responseObj Status] isEqual: @"Success"])
                    {
                        
                        [[DbAdapter getInstance] saveBookingsToLocal:responseObj];
                        
                        
                    }
                }
            }
            
            
        }];
        
        [req setFailedBlock:^{
            
            [AppLog Log:@"service error"];
            
            
        }];
        [req startSynchronous];
        [AppLog Log:@"Get bookings - end"];
        
    });
    
    dispatch_group_wait(group, DISPATCH_TIME_FOREVER);
    
    [activityIndicator stopAnimating];
    
    [sender performSegueWithIdentifier:@"goToHome" sender:sender];
    
    
}


@end
