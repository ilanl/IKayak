//
//  BookingViewController.h
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DbAdapter.h"

@interface BookingViewController : UIViewController<UITableViewDelegate, UITableViewDataSource>

@property (weak, nonatomic) IBOutlet UITableView *table;

@end
