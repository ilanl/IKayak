//
//  DbAdapter.h
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PreferenceResponse.h"
#import "BookingResponse.h"
#import "ForecastResponse.h"

#import "User.h"
#import "DayPref.h"
#import "AppLog.h"
#import "sqlite3.h"
#import "FMDatabase.h"
#import "FMDatabaseQueue.h"

@interface DbAdapter : NSObject{
    
}
@property BOOL isDirty;
@property (nonatomic, strong) User *currentUser;

+ (DbAdapter *)getInstance;

-(void)getCurrentUser;
-(void)createEditableCopyOfDatabaseIfNeeded;
-(BOOL)restoreDataBase;
-(void)savePreferencesToLocal:(PreferenceResponse*) response;
-(void)saveBookingsToLocal:(BookingResponse*) response;
-(void)saveForecastsToLocal:(ForecastResponse*) response;
-(void) saveUserToLocal: (NSString *) userEncodedName withPassword: (NSString *) encodedPassword;
-(void) updateUserState:(int) state;

-(NSMutableArray *) getAllKayaks;
-(NSMutableArray *) getKayaksByType:(int) type;
-(NSMutableArray *) getBookings;
-(NSMutableArray *) getForecasts;
-(int) getReminder;

-(NSArray *) getBookingsToCancel;
-(void) cleanUpUnActiveBookings;

-(NSMutableArray *) getTimes;
-(DayPref*) getDayOfWeek: (NSString *) dayOfWeek;

-(BOOL) updateBookingState:  (NSString *) key withState: (int) state;
-(BOOL) updateKayakPriority: (NSString *) key withPriority: (int) priority;
-(BOOL) updateTimeWithParams: (NSString *) dayOfWeek withTime: (int) time withType: (int) type;
-(BOOL) updateReminder:(int) reminderMins;
-(BOOL) updateDeviceToken:(NSString* ) deviceToken;
-(Set *) getLocalPreferencesFromDb;
-(NSString*)getDeviceToken;

@end
