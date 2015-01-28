//
//  DRCPrefsViewController.h
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DayDetailViewController.h"
#import "Global.h"
#import "DayPref.h"
#import "DbAdapter.h"

@interface SchedulingViewController : UIViewController <UITableViewDelegate, UITableViewDataSource>
@property (weak, nonatomic) IBOutlet UITableView *table;
-(DayPref*) getDay:(NSString*) dayOfWeek;
@property (nonatomic, strong)NSArray *daysOfWeek;
@property (nonatomic, strong)NSArray *dayPrefs;
@end
