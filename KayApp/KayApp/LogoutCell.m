//
//  LogoutCell.m
//  KayApp
//
//  Created by Ilan Levy on 10/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LogoutCell.h"
#import "DbAdapter.h"

@implementation LogoutCell
@synthesize btnLogout;
@synthesize parent;

-(id) init{

    self = [super init];
    
    if (self) {
        
        self.backgroundColor = [UIColor clearColor];
        
    }
    [btnLogout setFont:[UIFont fontWithName:@"Roboto-Bold" size:30.0]];
    
    return self;
}

- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];
    if (self) {
        // Initialization code
    }
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
