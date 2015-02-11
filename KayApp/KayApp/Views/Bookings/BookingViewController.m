//
//  BookingViewController.m
//  KayApp
//
//  Created by Ilan Levy on 10/11/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//
#import "Global.h"
#import "BookingCell.h"
#import "BookingViewController.h"
#import "TitleView.h"
#import "UIColor+Hex.h"

@interface BookingViewController ()

@end

@implementation BookingViewController
{
    NSMutableArray *bookings;
    
    NSArray *states;
}


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
    
    TitleView *iv = [[TitleView alloc] initWithFrame:CGRectMake(0, 0, 0, 0)];
    [iv setTitle:@"My Bookings" withTime:@""];
    iv.contentMode = UIViewContentModeCenter;
    [iv sizeToFit];
    
    self.navigationItem.titleView = iv;
    
    [[UITabBar appearance] setTintColor:[UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0]];
    
    [[UITabBar appearance] setBarTintColor:[UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0]];
    
    bookings = [[DbAdapter getInstance] getBookings];

    states = [NSArray arrayWithObjects: @"Booking-page-Approve-button.png", @"Booking-page-Cancel-Button.png",nil];
}

- (void)viewWillAppear:(BOOL)animated
{
    [[NSNotificationCenter defaultCenter]
     addObserver:self
     selector:@selector(applicationDidBecomeActiveNotification:)
     name:UIApplicationDidBecomeActiveNotification
     object:[UIApplication sharedApplication]];
    
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(reloadTable:) name:@"reloadTheTable" object:nil];
}

- (void)viewWillDisappear:(BOOL)animated {
    
    [[NSNotificationCenter defaultCenter]
     removeObserver:self
     name:UIApplicationDidBecomeActiveNotification
     object:[UIApplication sharedApplication]];

    [[NSNotificationCenter defaultCenter]
     removeObserver:self
     name:@"reloadTheTable"
     object:nil];
}

- (void)applicationDidBecomeActiveNotification:(NSNotification *)notification {
    // Do something here
    
    [self loaData];
}

- (void)reloadTable:(NSNotification *)notification {
    // Do something here
    
    [self loaData];
}


- (void)viewDidAppear:(BOOL)animated
{
    // Do any additional setup after loading the view, typically from a nib.
//    [self.navigationController setNavigationBarHidden:NO];
}


- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return [bookings count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    Booking *selectedBooking = [bookings objectAtIndex:indexPath.row];
    
    static NSString *simpleTableIdentifier = @"BookingCell";
    
    BookingCell *cell = (BookingCell *)[tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier];
    if (cell == nil)
    {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"BookingCell" owner:self options:nil];
        cell = [nib objectAtIndex:0];
    }
    
    cell.lblDay.text = selectedBooking.Day;
    cell.lblTime.text = selectedBooking.Time;
    cell.lblBoat.text = selectedBooking.KayakName;
    cell.selectedBooking = selectedBooking;
    cell.states = states;
    
    UIButton *btn = cell.btnState;
    [btn setTitle:[NSString stringWithFormat:@""] forState:UIControlStateNormal];
    [btn setFrame:CGRectMake(0, 0, 29, 29)];
    btn.tag = indexPath.row;
    [btn setBackgroundImage:[UIImage imageNamed: [states objectAtIndex:selectedBooking.State]] forState:UIControlStateNormal];
    
    [cell.lblDay setFont:[UIFont fontWithName:@"Roboto-Medium" size:17.0]];
    cell.lblDay.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    [cell.lblTime setFont:[UIFont fontWithName:@"Roboto-Medium" size:15.0]];
    cell.lblTime.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    [cell.lblBoat setFont:[UIFont fontWithName:@"Arial-Regular" size:15.0]];
    cell.lblBoat.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    [cell.btnBorder setBackgroundColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
    cell.btnLine.backgroundColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
   
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    return cell;
}

- (void)loaData
{
    bookings = [[DbAdapter getInstance] getBookings];
    [_table reloadData];
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
    return 60;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end