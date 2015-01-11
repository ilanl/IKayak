//
//  UIColor+Hex.h
//  Kayapp
//
//  Created by Ilan Levy on 11/13/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIColor (Hex)

+ (UIColor *)colorWithHexString:(NSString *)hex withAlpha:(CGFloat) alpha;

@end