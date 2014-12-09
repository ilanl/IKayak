//
//  DRCPrefsViewController.m
//  kayak
//
//  Created by Ilan Levy on 9/5/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "SchedulingViewController.h"
#import "DayCell.h"
#import "TitleView.h"
#import "UIColor+Hex.h"

@interface SchedulingViewController ()

@end

@implementation SchedulingViewController
@synthesize daysOfWeek;
@synthesize dayPrefs;
float dayCellHeight;

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    daysOfWeek = [NSArray arrayWithObjects:@"Sunday",@"Monday",@"Tuesday",@"Wednesday", @"Thursday", @"Friday", @"Saturday",nil];
    
    // Initialize table data
    dayPrefs = [[DbAdapter getInstance] getTimes];
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:@"Days" withTime:@""];
    iv.contentMode = UIViewContentModeCenter;
    [iv sizeToFit];
    
    self.navigationItem.titleView = iv;
    
    self.table.separatorInset = UIEdgeInsetsMake(0, 0, 0, 0);
    self.table.separatorStyle = UITableViewCellSeparatorStyleSingleLine;
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    CGFloat screenHeight = screenRect.size.height;
    dayCellHeight = (screenHeight-self.navigationController.navigationBar.frame.size.height-12)/ 7;
}

- (void) viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
}

- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender
{
    NSIndexPath *indexPath = [self.table indexPathForSelectedRow];
    NSString *dayOfWeek = [daysOfWeek objectAtIndex:indexPath.row];
    if ([segue.identifier isEqualToString:@"showDayDetail"])
    {
        DayPref *myDay = [[DbAdapter getInstance] getDayOfWeek:dayOfWeek];
        if (myDay == nil)
            myDay = [[DayPref alloc] initWithParams:dayOfWeek];
        
        DayDetailViewController *destViewController = segue.destinationViewController;
        [destViewController initializeViewWithDay:myDay];
        
        self.navigationItem.backBarButtonItem=[[UIBarButtonItem alloc] initWithTitle:@"" style:UIBarButtonItemStylePlain target:nil action:nil];
    }
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    [self performSegueWithIdentifier:@"showDayDetail" sender:nil];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return [daysOfWeek count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *simpleTableIdentifier = @"DayCell";
    
    DayCell *cell = (DayCell *)[tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier];
    if (cell == nil)
    {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"DayCell" owner:self options:nil];
        cell = [nib objectAtIndex:0];
    }
    
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    cell.lblDay.text = [daysOfWeek objectAtIndex:indexPath.row];
    [cell.lblDay setFont:[UIFont fontWithName:@"Roboto-Regular" size:19.0]];
    cell.lblDay.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    [cell.btnBorder setBackgroundColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
    cell.btnLine.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    cell.parent = self;
    
    return cell;
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return dayCellHeight;
}

-(DayPref*) getDay:(NSString*) dayOfWeek
{
    for (DayPref *d in dayPrefs) {
        if ([d.name isEqualToString:dayOfWeek])
            return d;
    }
    return [[DayPref alloc] initWithParams:dayOfWeek];
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}
@end
