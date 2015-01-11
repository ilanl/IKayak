//
//  BookingCell.m
//  KayApp
//
//  Created by Ilan Levy on 10/14/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "BookingCell.h"
#import "Booking.h"
#import "Global.h"

@implementation BookingCell

@synthesize lblBoat;
@synthesize lblDay;
@synthesize lblTime;
@synthesize btnState;
@synthesize selectedBooking;
@synthesize states;


- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];
    if (self) {
        // Initialization code
    }
    
    return self;
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated
{
    //[super setSelected:NO animated:animated];

    // Configure the view for the selected state
}

- (IBAction)btnStateChanged:(id)sender {
    UIButton *button = (UIButton *)sender;
    
    selectedBooking.State = ++selectedBooking.State % [states count];
    [button setBackgroundImage:[UIImage imageNamed: [states objectAtIndex:selectedBooking.State]] forState:UIControlStateNormal];
    
    //save to db
    [[DbAdapter getInstance] updateBookingState:selectedBooking.TripKey withState:selectedBooking.State];
}
@end
