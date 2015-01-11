//
//  TitleView.m
//  Kayapp
//
//  Created by Ilan Levy on 11/29/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "TitleView.h"
#import "UIColor+Hex.h"

@implementation TitleView
@synthesize lblHour,lblDay;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"TitleView" owner:self options:nil];
        self = [nib objectAtIndex:0];
        
        self.frame = CGRectMake(0,0, 300, 32);
        
        self.userInteractionEnabled = YES;
        NSLog(@"titleview control loaded");
        
    }
    
    return self;
}

-(void) setTitle:(NSString*) day withTime:(NSString*) hour{
    
    lblDay.font = [UIFont fontWithName:@"Roboto-Medium" size:23];
    lblDay.textColor = [UIColor whiteColor];
    lblDay.text = day;
    
    lblHour.font = [UIFont fontWithName:@"Roboto-Light" size:19];
    lblHour.textColor = [UIColor whiteColor];
    lblHour.text = hour;
    
    NSLog(@"day: %@ hour: %@", day, hour);
}
@end
