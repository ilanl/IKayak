//
//  UIColor+Hex.m
//  Kayapp
//
//  Created by Ilan Levy on 11/13/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "UIColor+Hex.h"

@implementation UIColor (Hex)
+ (UIColor *)colorWithHexString:(NSString *)hex withAlpha:(CGFloat) alpha
{
    if ([hex length]!=6 && [hex length]!=3)
    {
        return nil;
    }
    
    NSUInteger digits = [hex length]/3;
    CGFloat maxValue = (digits==1)?15.0:255.0;
    
    NSUInteger redHex = 0;
    NSUInteger greenHex = 0;
    NSUInteger blueHex = 0;
    
    sscanf([[hex substringWithRange:NSMakeRange(0, digits)] UTF8String], "%x", &redHex);
    sscanf([[hex substringWithRange:NSMakeRange(digits, digits)] UTF8String], "%x", &greenHex);
    sscanf([[hex substringWithRange:NSMakeRange(2*digits, digits)] UTF8String], "%x", &blueHex);
    
    CGFloat red = redHex/maxValue;
    CGFloat green = greenHex/maxValue;
    CGFloat blue = blueHex/maxValue;
    
    return [UIColor colorWithRed:red green:green blue:blue alpha:alpha];
}

@end
