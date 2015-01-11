//
//  DRCBoatsViewController.h
//  kayak
//
//  Created by Ilan Levy on 9/6/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DbAdapter.h"
@interface RankViewController : UIViewController <UITableViewDelegate, UITableViewDataSource>

@property (weak, nonatomic) IBOutlet UITableView *table;

@property (nonatomic, strong) NSMutableArray *boats;

@end
