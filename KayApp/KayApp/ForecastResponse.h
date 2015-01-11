//
//  ForecastResponse.h
//  KayApp
//
//  Created by Ilan Levy on 10/16/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Forecast.h"
#import "BaseResponse.h"

@protocol Forecast;

@interface ForecastResponse : BaseResponse

@property (nonatomic, strong) NSMutableArray<Forecast> *Forecasts;

@end
