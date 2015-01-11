//
//  LogoutCell.m
//  KayApp
//
//  Created by Ilan Levy on 10/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LogoutCell.h"
#import "DbAdapter.h"
#import "UIColor+Hex.h"

@implementation LogoutCell
@synthesize btnLogout;
@synthesize parent;


- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"SettingCell" owner:self options:nil];
        self = [nib objectAtIndex:0];
    }
    
    self.backgroundView.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    self.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];

    return self;
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated
{
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

- (IBAction)logoutPressed:(id)sender {

    //exit(0); //TODO: need to save all
    
    BOOL success = [[DbAdapter getInstance] restoreDataBase];
    NSLog(@"logout pressed");
    if (success)
        [parent performSegueWithIdentifier:@"goToSignIn" sender:parent];

}
@end
