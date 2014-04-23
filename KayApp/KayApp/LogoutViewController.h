//
//  LogoutViewController.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DbAdapter.h"

@interface LogoutViewController : UIViewController<UITableViewDelegate, UITableViewDataSource>
{
    NSMutableArray *dataArray;
}

@property (weak, nonatomic) IBOutlet UITableView *mainTable;

@end
