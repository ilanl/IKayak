//
//  WeatherClient.m
//  KayApp
//
//  Created by Ilan Levy on 10/14/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "Weather.h"

@implementation Weather

static Weather *instance = NULL;
@synthesize lastViewed;

-(id) init
{
    self = [super init];
    
    if (self) {
        
    }
    
    return self;
}

-(void) reload
{
    [AppLog Log:@"loading forecasts"];
    
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://breezback.com/IKayak/forecasts.ashx"]];
    
    __weak ASIHTTPRequest *req = [ASIHTTPRequest requestWithURL:url];
    
    [req addRequestHeader:@"User-Agent" value:@"ASIHTTPRequest"];
    [req addRequestHeader:@"Content-Type" value:@"application/json"];
    
    [req setTimeOutSeconds:60];
    [req startSynchronous];
        
    NSString *responseString = [req responseString];
    NSError* err;
    ForecastResponse *responseObj = [[ForecastResponse alloc] initWithString:responseString error:&err];
    if (!err)
    {
        if ([[responseObj Status] isEqual: @"Success"])
        {
            [[DbAdapter getInstance] saveForecastsToLocal:responseObj];
            
            [self setLastViewed:[NSDate date]];
            
            [AppLog Log:@"Forecasts retrieved"];
        }
    }
}

+(Weather *)getInstance {
    @synchronized(self) {
        if (instance == NULL) {
            instance = [[self alloc] init];
        }
    }
    return instance;
}


@end
