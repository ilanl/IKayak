//
//  MenuView.h
//  KayApp
//
//  Created by Ilan Levy on 10/17/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface MenuView : UIView
@property (weak, nonatomic) IBOutlet UIImageView *imgBox;

- (IBAction)openGatePressed:(id)sender;
- (IBAction)openDaysPressed:(id)sender;
- (IBAction)openBoatsPressed:(id)sender;
- (IBAction)openBookingsPressed:(id)sender;
- (IBAction)openSettingsPressed:(id)sender;
- (IBAction)openFacebookPressed:(id)sender;

@property (nonatomic, retain) UIViewController *parent;

@property (weak, nonatomic) IBOutlet UILabel *lblDays;
@property (weak, nonatomic) IBOutlet UILabel *lblSettings;
@property (weak, nonatomic) IBOutlet UILabel *lblSocial;
@property (weak, nonatomic) IBOutlet UILabel *lblBookings;
@property (weak, nonatomic) IBOutlet UILabel *lblBoats;
@property (weak, nonatomic) IBOutlet UILabel *lblOpenGate;

@end
