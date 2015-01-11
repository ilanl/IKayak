//
//  DRCDayDetailViewController.h
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DayPref.h"
#import "Global.h"
#import "DbAdapter.h"

@interface DayDetailViewController : UIViewController

@property (strong, nonatomic) DayPref *pref;

-(void) initializeViewWithDay: (DayPref *) day;

@end
