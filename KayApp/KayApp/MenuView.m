//
//  MenuView.m
//  KayApp
//
//  Created by Ilan Levy on 10/17/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "MenuView.h"

@implementation MenuView

@synthesize parent;
@synthesize imgBox;
@synthesize lblBoats,lblSocial,lblSettings,lblOpenGate,lblDays,lblBookings;

static NSString *phoneNumber = NULL;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"Menu" owner:self options:nil];
        self = [nib objectAtIndex:0];
        
        self.frame = CGRectMake(0,230, self.bounds.size.width-10, self.bounds.size.height);
        //imgBox.image = [UIImage imageNamed:@"MenuBox"];
        // load the sub view from another NIB
        //self.backgroundColor = [UIColor redColor];
        
        phoneNumber = [@"tel://" stringByAppendingString:
         [[NSBundle mainBundle] objectForInfoDictionaryKey:@"tel_gate"]];
      
        [lblBoats setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        [lblBookings setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        [lblDays setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        [lblOpenGate setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        [lblSettings setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        [lblSocial setFont:[UIFont fontWithName:@"Roboto-Regular" size:16.0]];
        
        
    }
    return self;
}

- (IBAction)openGatePressed:(id)sender {
    
    NSLog(@"calling gate tel:");
    
    NSURL *phoneNumberStr = [NSURL URLWithString:phoneNumber];
    [[UIApplication sharedApplication] openURL: phoneNumberStr];
}

- (IBAction)openDaysPressed:(id)sender {
    [parent performSegueWithIdentifier:@"goToSchedulings" sender:parent];
}

- (IBAction)openBoatsPressed:(id)sender {
    [parent performSegueWithIdentifier:@"goToRankings" sender:parent];
}

- (IBAction)openBookingsPressed:(id)sender {
    [parent performSegueWithIdentifier:@"goToBookings" sender:parent];
}

- (IBAction)openSettingsPressed:(id)sender {
    [parent performSegueWithIdentifier:@"goToSettings" sender:parent];
}
@end
