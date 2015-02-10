//
//  LogoutCell.h
//  KayApp
//
//  Created by Ilan Levy on 10/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LogoutCell : UITableViewCell

- (IBAction)logoutPressed:(id)sender;
@property (nonatomic, strong) UIViewController* parent;
@property (weak, nonatomic) IBOutlet UIButton *btnLogout;

@end
