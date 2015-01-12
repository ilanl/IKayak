//
//  SettingCell.h
//  Kayapp
//
//  Created by Ilan Levy on 1/2/14.
//  Copyright (c) 2014 Ilan Levy. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface SettingCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *lblName;
@property (weak, nonatomic) IBOutlet UIButton *btnBorder;
@property (weak, nonatomic) IBOutlet UIButton *btnLine;
@property (weak, nonatomic) IBOutlet UIButton *btnState;

-(void) updateMark:(BOOL) isSelected;

@end
