//
//  WeatherClient.h
//  KayApp
//
//  Created by Ilan Levy on 10/14/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AppLog.h"
#import "ASIHTTPRequest.h"
#import "ForecastResponse.h"
#import "DbAdapter.h"

@protocol WeatherHttpClientDelegate;

@interface Weather : NSObject

@property (nonatomic, strong) NSDate *lastViewed;

+ (Weather *)getInstance;

- (void)reload;


@end
