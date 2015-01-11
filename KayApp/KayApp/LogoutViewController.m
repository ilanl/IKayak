//
//  LogoutViewController.m
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "LogoutViewController.h"
#import "AppDelegate.h"
#import "LogoutCell.h"
#import "UIColor+Hex.h"
#import "TitleView.h"
#import "SettingCell.h"
#import "Util.h"

@interface LogoutViewController ()

@end

@implementation LogoutViewController
@synthesize mainTable;
NSIndexPath* checkedStatusIndexPath;
NSIndexPath* checkedReminderIndexPath;
static NSString *logoutCellIdentifier = @"LogoutCell";
static NSString *settingCellIdentifier = @"SettingCell";

NSInteger sectionOfTheCell, rowOfTheCell;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.view.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    mainTable.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:@"Account" withTime:@""];
    iv.contentMode = UIViewContentModeCenter;
    [iv sizeToFit];
    self.navigationItem.titleView = iv;
    
	// Do any additional setup after loading the view.
    [Reminder getInstance];
    
    //Initialize the dataArray
    dataArray = [[NSMutableArray alloc] init];
    
    //First section data
    NSArray *firstItemsArray = [[NSArray alloc] initWithObjects:@"Yeah, book for me", @"No, not in the mood", nil];
    NSDictionary *firstItemsArrayDict = [NSDictionary dictionaryWithObject:firstItemsArray forKey:@"data"];
    [dataArray addObject:firstItemsArrayDict];
    
    //Second section data
    NSArray *secondItemsArray = [[NSArray alloc] initWithObjects: @"None", @"30 mins before", @"45 mins before", @"1 hour before", @"8 hours before", nil];
    NSDictionary *secondItemsArrayDict = [NSDictionary dictionaryWithObject:secondItemsArray forKey:@"data"];
    [dataArray addObject:secondItemsArrayDict];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return [dataArray count] + 1;
}

- (CGFloat) tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section
{
    if (section == 1)
        return 70.0;
    
    return 44.0;
}

- (UIView *)tableView:(UITableView *)tableView viewForFooterInSection:(NSInteger)section
{
    if (section != 0)
        return nil;
    
    // create the parent view that will hold header Label
    UIView* customView = [[UIView alloc] initWithFrame:CGRectMake(10.0, 0.0, 300.0, 60.0)];
    
    // create the button object
    UILabel * footerLabel = [[UILabel alloc] initWithFrame:CGRectZero];
    footerLabel.opaque = NO;
    footerLabel.textColor = [UIColor whiteColor];
    footerLabel.font =[UIFont fontWithName:@"Roboto-Thin" size:13.0];
    footerLabel.frame = CGRectMake(10.0, 0.0, 300.0, 44.0);
    footerLabel.lineBreakMode = NSLineBreakByWordWrapping;
    footerLabel.numberOfLines = 3;
    
	footerLabel.text=@"This will automatically cancel all active and further bookings";
    
    [customView addSubview:footerLabel];
    
    return customView;

}

- (UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section
{
    // create the parent view that will hold header Label
    UIView* customView = [[UIView alloc] initWithFrame:CGRectMake(10.0, 0.0, 300.0, 50.0)];
    
    // create the button object
    UILabel * headerLabel = [[UILabel alloc] initWithFrame: CGRectMake(0, 5, 140, 50)];
    headerLabel.opaque = NO;
    headerLabel.textColor = [UIColor whiteColor];
    headerLabel.font =[UIFont fontWithName:@"Roboto-Regular" size:18.0];
    headerLabel.frame = CGRectMake(10.0, 0.0, 300.0, 44.0);
    
    // If you want to align the header text as centered
    // headerLabel.frame = CGRectMake(150.0, 0.0, 300.0, 44.0);
    
    if(section == 0)
		headerLabel.text = @"Status";
    else if(section == 1){
        headerLabel.frame = CGRectMake(10.0, 5.0, 320.0, 90.0);
		headerLabel.text = @"Set reminder";
    }
    else
        return nil;

    [customView addSubview:headerLabel];
    
    return customView;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    
    if (section == 2)
        return 1;
    
    //Number of rows it should expect should be based on the section
    NSDictionary *dictionary = [dataArray objectAtIndex:section];
    NSArray *array = [dictionary objectForKey:@"data"];
    return [array count];
}


-(SettingCell *) drawSettingCell: (NSIndexPath *)indexPath withTable: (UITableView *) tableView
{
    User *user = [[DbAdapter getInstance] currentUser];
    
    SettingCell *cell = (SettingCell *)[tableView dequeueReusableCellWithIdentifier:settingCellIdentifier];
    
    if (cell == nil)
    {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"SettingCell" owner:self options:nil];
        cell = [nib objectAtIndex:0];
        
        NSDictionary *dictionary = [dataArray objectAtIndex:sectionOfTheCell];
        NSArray *array = [dictionary objectForKey:@"data"];
        NSString *cellValue = [array objectAtIndex:rowOfTheCell];
        cell.lblName.text = cellValue;
        cell.selectionStyle = UITableViewCellSelectionStyleNone;
        UIButton *button = cell.btnState;
        [button addTarget:self action:@selector(checkButtonTapped:event:) forControlEvents:UIControlEventTouchUpInside];
        
    }
    
    if (sectionOfTheCell == 0){
        
        if (rowOfTheCell == 0 && user.state == 0)
        {
            [cell updateMark: TRUE];
            checkedStatusIndexPath = indexPath;
        }
        else if (rowOfTheCell == 1 && user.state == 1)
        {
            [cell updateMark: TRUE];
            checkedStatusIndexPath = indexPath;
        }
        else{
            [cell updateMark: FALSE];
        }
    }
    
    else if (sectionOfTheCell == 1){
        
        int reminder = user.reminder;
        if (rowOfTheCell == 0 && reminder == 0)
        {
            [cell updateMark: TRUE];
            checkedReminderIndexPath = indexPath;
        }
        else if (rowOfTheCell == 1 && reminder == 30)
        {
            [cell updateMark: TRUE];
            checkedReminderIndexPath = indexPath;
        }
        else if (rowOfTheCell == 2 && reminder == 45)
        {
            [cell updateMark: TRUE];
            checkedReminderIndexPath = indexPath;
        }
        else if (rowOfTheCell == 3 && reminder == 60)
        {
            [cell updateMark: TRUE];
            checkedReminderIndexPath = indexPath;
        }
        else if (rowOfTheCell == 4 && reminder == 480)
        {
            [cell updateMark: TRUE];
            checkedReminderIndexPath = indexPath;
        }
        else {
            [cell updateMark: FALSE];
        }
    }
    
    return cell;


}

-(LogoutCell *) drawLogoutCell: (NSIndexPath *)indexPath withTable: (UITableView *) tableView
{
    [AppLog Log:@"draw logout section: %i , row:%i", sectionOfTheCell, rowOfTheCell];
    
    LogoutCell *logoutCell = (LogoutCell *)[tableView dequeueReusableCellWithIdentifier:logoutCellIdentifier];
    if (logoutCell == nil)
    {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"LogoutCell" owner:self options:nil];
        logoutCell = [nib objectAtIndex:0];
    }
    
    return logoutCell;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    sectionOfTheCell = [indexPath section];
    rowOfTheCell = [indexPath row];
    
    [AppLog Log: [NSString stringWithFormat:@"section: %li row: %li", (long)sectionOfTheCell, (long)rowOfTheCell]];
    
    switch (sectionOfTheCell) {
        case 0:
        case 1:
            return [self drawSettingCell: indexPath withTable:tableView];
            break;
        case 2:
            return [self drawLogoutCell: indexPath withTable:tableView];
            break;
        default:
            break;
    }
    return nil;
}


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    
    return 50;
    
    
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    
    if ((indexPath.section == checkedStatusIndexPath.section && indexPath.row == checkedStatusIndexPath.row) || (indexPath.section == checkedReminderIndexPath.section && indexPath.row == checkedReminderIndexPath.row) || indexPath.section == 2)
        return;
    
    if (indexPath != nil)
	{
		[self accessoryButtonTappedForRowWithIndexPath:self.mainTable indexPath: indexPath ];
	}
}

- (void)checkButtonTapped:(id)sender event:(id)event
{
	NSSet *touches = [event allTouches];
	UITouch *touch = [touches anyObject];
	CGPoint currentTouchPosition = [touch locationInView:self.mainTable];
    
	NSIndexPath *indexPath = [self.mainTable indexPathForRowAtPoint: currentTouchPosition];
	if (indexPath != nil)
	{
		[self accessoryButtonTappedForRowWithIndexPath:self.mainTable indexPath: indexPath ];
	}
}

- (void) accessoryButtonTappedForRowWithIndexPath:(UITableView *)tableView indexPath: (NSIndexPath *)indexPath
{
    if (indexPath.section == 0)
    {
        if(checkedStatusIndexPath)
        {
            SettingCell *uncheckCell = (SettingCell *)[tableView
                                            cellForRowAtIndexPath:checkedStatusIndexPath];
            [uncheckCell updateMark:FALSE];
        }
        SettingCell *checkCell = (SettingCell *)[tableView
                                        cellForRowAtIndexPath:indexPath];
        [checkCell updateMark:TRUE];
        
        checkedStatusIndexPath = indexPath;
        
        [[DbAdapter getInstance] updateUserState:checkedStatusIndexPath.row == 1 ? 1 : 0];
        [[Global sharedSingleton] setStatusHasChanged:true];
    }
    if (indexPath.section == 1)
    {
        if(checkedReminderIndexPath)
        {
            SettingCell *uncheckCell = (SettingCell *)[tableView
                                                       cellForRowAtIndexPath:checkedReminderIndexPath];
            [uncheckCell updateMark:FALSE];
        }
        SettingCell *checkCell = (SettingCell *)[tableView
                                                 cellForRowAtIndexPath:indexPath];
        [checkCell updateMark:TRUE];
        
        switch (indexPath.row) {
            case 0:
                [[DbAdapter getInstance] updateReminder:0];
                break;
            case 1:
                [[DbAdapter getInstance] updateReminder:30];
                break;
            case 2:
                [[DbAdapter getInstance] updateReminder:45];
                break;
            case 3:
                [[DbAdapter getInstance] updateReminder:60];
                break;
            case 4:
                [[DbAdapter getInstance] updateReminder:480];
                break;
            default:
                break;
        }
        checkedReminderIndexPath = indexPath;
    }
    [[DbAdapter getInstance] getCurrentUser];
}

- (void)viewDidAppear:(BOOL)animated
{
    // Do any additional setup after loading the view, typically from a nib.
//    [self.navigationController setNavigationBarHidden:NO];
}

@end
