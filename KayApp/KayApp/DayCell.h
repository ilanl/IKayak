//
//  DayCell.h
//  Kayapp
//
//  Created by Ilan Levy on 11/30/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "SchedulingViewController.h"

@interface DayCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *lblDay;
@property (weak, nonatomic) IBOutlet UIButton *btnSelect;
@property (weak, nonatomic) IBOutlet UIButton *btnBorder;
@property (weak, nonatomic) IBOutlet UIButton *btnLine;
@property (weak, nonatomic) IBOutlet SchedulingViewController *parent;
- (IBAction)didSelectDay:(id)sender;

@end
