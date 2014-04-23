//
//  DayCell.m
//  Kayapp
//
//  Created by Ilan Levy on 11/30/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "DayCell.h"
#import "UIColor+Hex.h"

@implementation DayCell
@synthesize lblDay, btnSelect, btnBorder, btnLine;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"DayCell" owner:self options:nil];
        self = [nib objectAtIndex:0];
    }
    
    return self;
}

@end
