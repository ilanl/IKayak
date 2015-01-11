//
//  BoatCell.h
//  KayApp
//
//  Created by Ilan Levy on 10/20/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DbAdapter.h"
#import "BoatPref.h"

@interface BoatCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *lblBoat;

@property (weak, nonatomic) IBOutlet UIButton *btnLine;
@property (weak, nonatomic) IBOutlet UIButton *btnBorder;
- (IBAction)btnPriorityChanged:(id)sender;

@property (weak, nonatomic) IBOutlet UIButton *btnPriority;

@property (weak, nonatomic) BoatPref *selectedBoatPref;
@property NSArray *priorities;


@end
