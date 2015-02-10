//
//  BookingCell.h
//  KayApp
//
//  Created by Ilan Levy on 10/14/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Booking.h"
#import "DbAdapter.h"

@interface BookingCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *lblBoat;
@property (weak, nonatomic) IBOutlet UIButton *btnLine;
@property (weak, nonatomic) IBOutlet UIButton *btnState;
@property (weak, nonatomic) IBOutlet UILabel *lblDay;
@property (weak, nonatomic) IBOutlet UILabel *lblTime;
@property (weak, nonatomic) IBOutlet UIButton *btnBorder;
- (IBAction)btnStateChanged:(id)sender;
@property (weak, nonatomic) Booking *selectedBooking;
@property NSArray *states;
@end 
