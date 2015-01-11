//
//  BoatCell.m
//  KayApp
//
//  Created by Ilan Levy on 10/20/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BoatCell.h"

@implementation BoatCell

@synthesize lblBoat, btnBorder;
@synthesize btnPriority;
@synthesize selectedBoatPref;
@synthesize priorities;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"BoatCell" owner:self options:nil];
        self = [nib objectAtIndex:0];
    }
    
    return self;
}

- (IBAction)btnPriorityChanged:(id)sender {
    
    [AppLog Log:@"updating priority"];
    
    UIButton *button = (UIButton *)sender;
    selectedBoatPref.priority = --selectedBoatPref.priority % [priorities count];
    [button setBackgroundImage:[UIImage imageNamed: [priorities objectAtIndex:selectedBoatPref.priority]] forState:UIControlStateNormal];
    
    //save to db
    [[DbAdapter getInstance] updateKayakPriority:selectedBoatPref.key withPriority:selectedBoatPref.priority];

}

- (void)layoutSubviews
{
    [super layoutSubviews];
    
    [self.contentView layoutIfNeeded];
}
@end
