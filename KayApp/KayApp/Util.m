//
//  Util.m
//  Kayapp
//
//  Created by Ilan Levy on 1/4/14.
//  Copyright (c) 2014 Ilan Levy. All rights reserved.
//

#import "Util.h"

@implementation Util

+ (BOOL)isiOSVerGreaterThen7 {
    if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_6_1) {
        return NO;
    } else {
        return YES;
    }
}

@end
