//
//  DRCBoatsViewController.m
//  kayak
//
//  Created by Ilan Levy on 9/6/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "RankViewController.h"
#import "BoatPref.h"
#import "Global.h"
#import "BoatCell.h"
#import "UIColor+Hex.h"
#import "TitleView.h"

@interface RankViewController ()

@end

@implementation RankViewController
{
    NSArray *priorities;
}

@synthesize boats;

static NSString *boatCellIdentifier = @"BoatCell";

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
        self.navigationItem.backBarButtonItem.title = @" ";
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Initialize table data
    priorities = [NSArray arrayWithObjects:@"plus.png",@"3.png",@"2.png",@"1.png", nil];
    
    self.table.rowHeight = 60;
    self.table.separatorInset = UIEdgeInsetsMake(0, 0, 0, 0);
    self.table.separatorStyle = UITableViewCellSeparatorStyleSingleLine;
}

- (void)viewDidAppear:(BOOL)animated
{
    // Do any additional setup after loading the view, typically from a nib.
//    [self.navigationController setNavigationBarHidden:NO];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return [boats count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    BoatPref *selectedBoatPref = [boats objectAtIndex:indexPath.row];
    
    BoatCell *cell = (BoatCell *)[tableView dequeueReusableCellWithIdentifier:boatCellIdentifier];
    if (cell == nil)
    {
        NSArray *nib = [[NSBundle mainBundle] loadNibNamed:@"BoatCell" owner:self options:nil];
        cell = [nib objectAtIndex:0];
    }
    
    cell.lblBoat.text = selectedBoatPref.name;
    cell.selectedBoatPref = selectedBoatPref;
    cell.priorities = priorities;
    [cell.contentView setUserInteractionEnabled: NO];
    UIButton *btn = cell.btnPriority;
    [btn setTitle:[NSString stringWithFormat:@""] forState:UIControlStateNormal];
    [btn setFrame:CGRectMake(0, 0, 32, 32)];
    btn.tag = indexPath.row;
    [btn setBackgroundImage:[UIImage imageNamed: [priorities objectAtIndex:selectedBoatPref.priority]] forState:UIControlStateNormal];
    
    cell.selectionStyle = UITableViewCellSelectionStyleNone;
    //[cell.lblBoat setFont:[UIFont fontWithName:@"Roboto-Regular" size:19.0]];
    cell.lblBoat.textColor = [UIColor colorWithHexString:@"2C2C2C" withAlpha:1.0];
    
    [cell.btnBorder setBackgroundColor:[UIColor colorWithHexString:@"1593DB" withAlpha:1.0]];
    
    return cell;
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
