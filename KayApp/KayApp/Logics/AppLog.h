//
//  Logger.h
//  KayApp
//
//  Created by Ilan Levy on 10/15/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface AppLog : NSObject

@property (nonatomic) NSString* level;

+ (void)Log:(NSString* )format, ...;

@end
