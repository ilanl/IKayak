//
//  SettingCell.m
//  Kayapp
//
//  Created by Ilan Levy on 1/2/14.
//  Copyright (c) 2014 Ilan Levy. All rights reserved.
//

#import "SettingCell.h"
#import "UIColor+Hex.h"

@implementation SettingCell

@synthesize lblName, btnState;
@synthesize btnLine, btnBorder;


- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"SettingCell" owner:self options:nil];
        self = [nib objectAtIndex:0];
    }
    
    [lblName setFont:[UIFont fontWithName:@"Roboto-Medium" size:17.0]];
    lblName.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    [btnBorder setBackgroundColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
    [btnLine setBackgroundColor:[UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0]];
    
    self.selectionStyle = UITableViewCellSelectionStyleNone;
    self.accessoryType = UITableViewCellAccessoryNone;
    
    return self;
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated
{
    //[super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

-(void) updateMark:(BOOL) isSelected{
    //draw the appropriate mark
    
    if (isSelected == TRUE){
         [btnState setBackgroundImage:[UIImage imageNamed:@"Booking-page-Approve-button"] forState:UIControlStateNormal];
    }
    else{
        
        [btnState setBackgroundImage:nil forState:UIControlStateNormal];
        
        //[btnState setBackgroundImage:[UIImage imageNamed:@"Booking-page-Cancel-Button"] forState:UIControlStateNormal];
    }
    
}

@end
