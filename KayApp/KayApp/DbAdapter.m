//
//  DbAdapter.m
//  KayApp
//
//  Created by Ilan Levy on 9/21/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "DbAdapter.h"
#import "BoatPref.h"
#import "DayPref.h"
#import "Booking.h"


@implementation DbAdapter
static DbAdapter *instance = NULL;

@synthesize currentUser;
@synthesize isDirty;
NSString *path;

static FMDatabaseQueue *_queue;

-(id)init
{
    self = [super init];
    if (self){
        isDirty = FALSE;
        currentUser = [[User alloc]init];
        
        [self createEditableCopyOfDatabaseIfNeeded];
        _queue = [FMDatabaseQueue databaseQueueWithPath:path];
        [self getCurrentUser];
    }
    return self;
}

+(DbAdapter *)getInstance {
    @synchronized(self) {
        if (instance == NULL) {
            instance = [[self alloc] init];
        }
    }
    return instance;
}

- (void)createEditableCopyOfDatabaseIfNeeded
{
    [AppLog Log:@"Checking for database file"];
	NSFileManager *fileManager = [NSFileManager defaultManager];
	NSError *error;
	NSString *dbPath = [self getDBPath];
	NSString *defaultDBPath = [[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:@"drc.sqlite"];
	
	
	if(![fileManager fileExistsAtPath:dbPath])
    {
		[AppLog Log:@"copying fresh sqlite file..."];
		BOOL copySuccess = [fileManager copyItemAtPath:defaultDBPath toPath:dbPath error:&error];
		if (!copySuccess) {
			NSAssert1(0, @"Failed to create writable database file with message '%@'.", [error localizedDescription]);
		}
	}
    
    path = dbPath;
    
}

- (NSString *) getDBPath {
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory , NSUserDomainMask, YES);
	NSString *documentsDir = [paths objectAtIndex:0];
	return [documentsDir stringByAppendingPathComponent:@"drc.sqlite"];
}

- (void)getCurrentUser
{
    [_queue inDatabase:^(FMDatabase *db)
    {
        FMResultSet *results = [db executeQuery:@"SELECT name,password,state,reminder FROM User"];
        if ([results next])
        {
            if (currentUser == nil)
                currentUser = [[User alloc] init];
            
            currentUser.name = [results stringForColumnIndex:0];
            currentUser.password = [results stringForColumnIndex:1];
            currentUser.state = [results intForColumnIndex:2];
            currentUser.reminder = [results intForColumnIndex:3];
        }
        [results close];
    }];
}


-(BOOL)restoreDataBase
{
    //delete all db
    NSFileManager *fileManager = [NSFileManager defaultManager];
    NSError *error;
    
    if ([fileManager isDeletableFileAtPath:path]) {
        BOOL success = [fileManager removeItemAtPath:path error:&error];
        if (!success) {
            [AppLog Log:@"Error removing file at path: %@", error.localizedDescription, nil];
            return FALSE;
        }
        [self createEditableCopyOfDatabaseIfNeeded];
        
    }
    return TRUE;
}


- (void)extractKayaksToDb:(PreferenceResponse *)response
{
    [_queue inTransaction:^(FMDatabase *db, BOOL *rollback)
    {
        [db executeUpdate:@"DELETE FROM Kayak"];
        [AppLog Log:@"Delete kayaks successfully"];
    
        for (LightKayakPref* kp in response.Set.KayakPrefs)
        {
            [db executeUpdate:@"INSERT INTO Kayak(Key,Name,Type,Weight) VALUES(?,?,?,?)",[NSString stringWithFormat:@"%@", kp.Key],[NSString stringWithFormat:@"%@", kp.Name], [NSNumber numberWithInt:kp.Type], [NSNumber numberWithInt:kp.Weight], nil];
            
            [AppLog Log:@"Inserting kayak: %@", kp.Name];
        }
    }];
}

- (void)extractBookingsToDb:(BookingResponse *)response
{
    [_queue inTransaction:^(FMDatabase *db, BOOL *rollback)
    {
        [db executeUpdate:@"DELETE FROM Booking"];
        [AppLog Log:@"Delete bookings successfully"];
    
        for (Booking* b in response.Bookings)
        {
            [db executeUpdate:@"INSERT INTO Booking(Name,Type,Day,Time,State,TripKey,OutingDate) VALUES(?,?,?,?,?,?,?)",
             [NSString stringWithFormat:@"%@", b.KayakName],
             [NSNumber numberWithInt:b.Type],
             [NSString stringWithFormat:@"%@", b.Day],
             [NSString stringWithFormat:@"%@", b.Time],
             [NSNumber numberWithInt:0],
             [NSString stringWithFormat:@"%@", b.TripKey],
             [NSString stringWithFormat:@"%@", b.OutingDate], nil];
            
            [AppLog Log:@"Inserting kayak: %@", b.KayakName];
        }
    }];
}

- (void)extractTimesToDb:(PreferenceResponse *)response
{
    [_queue inTransaction:^(FMDatabase *db, BOOL *rollback)
     {
        [db executeUpdate:@"DELETE FROM Time"];
        [AppLog Log:@"Delete Time prefs successfully"];
        for (LightTimePref* t in response.Set.TimePrefs)
        {
            [db executeUpdate:@"INSERT INTO Time(DayOfWeek,Time,Type) VALUES(?,?,?)",
             [NSString stringWithFormat:@"%@", t.DayOfWeek],
             [NSNumber numberWithInt:t.Time],
             [NSNumber numberWithInt:t.Type], nil];
            [AppLog Log:@"Inserting time: %@", t.DayOfWeek];
        }
     }];
}

-(void) cleanUpUnActiveBookings
{
    [_queue inDatabase:^(FMDatabase *db)
     {
         [db executeUpdate:@"DELETE FROM Booking WHERE State > 0"];
     }];
    [AppLog Log:@"Delete un-active bookings successfully"];
}

-(void)saveForecastsToLocal:(ForecastResponse*) response
{
    // Assume you have a 'date'
    NSCalendar *gregorianCal = [[NSCalendar alloc] initWithCalendarIdentifier:NSGregorianCalendar];
//    NSDateComponents *dateComps = [gregorianCal components: (NSHourCalendarUnit | NSMinuteCalendarUnit) fromDate:[NSDate date]];
    
    [_queue inTransaction:^(FMDatabase *db, BOOL *rollback)
     {
         [db executeUpdate:@"DELETE FROM Forecast"];
     
         for (Forecast* f in response.Forecasts)
         {
//             long localHourInt = [dateComps hour];
//             int hourInt = [[f.Hour substringToIndex:4] intValue];
//             if (hourInt < localHourInt)
//                 continue;
             
             [db executeUpdate:@"INSERT INTO Forecast(Day,Hour,TempC,WaterTempC,WaveH,Weather,SwellSecs,WindDir,WindF) VALUES(?,?,?,?,?,?,?,?,?)",
                 [NSString stringWithFormat:@"%@", f.Day],
                 [NSString stringWithFormat:@"%@", f.Hour],
                 [NSString stringWithFormat:@"%@", f.TempC],
                 [NSString stringWithFormat:@"%@", f.WaterTempC],
                 [NSString stringWithFormat:@"%@", f.WaveH],
                 [NSString stringWithFormat:@"%@", f.Weather],
                 [NSString stringWithFormat:@"%@", f.SwellSecs],
                 [NSString stringWithFormat:@"%@", f.WindDir],
                 [NSString stringWithFormat:@"%@", f.WindF],nil];
         
             [AppLog Log:@"Added Forecast: %@", f.Day];
         }
     }];
}

-(void) savePreferencesToLocal:(PreferenceResponse*) response
{
    [self extractUserSettingsToDb:response];
    [self extractKayaksToDb:response];
    [self extractTimesToDb:response];
}

-(void) saveBookingsToLocal:(BookingResponse*) response
{
    [self extractBookingsToDb:response];
}

-(void) saveUserToLocal: (NSString *) encodedName withPassword: (NSString *) password
{
    [_queue inDatabase:^(FMDatabase *db)
     {
         [db executeUpdate:@"INSERT OR REPLACE INTO User (Name, Password) VALUES (?,?)",
          [NSString stringWithFormat:@"%@", encodedName],
          [NSString stringWithFormat:@"%@", password], nil];
     }];
}

-(NSMutableArray *) getBookings
{
    NSMutableArray *bookingsArray = [[NSMutableArray alloc] init];
    [_queue inDatabase:^(FMDatabase *db)
     {
         FMResultSet *results = [db executeQuery:@"SELECT Name, Type, Day, Time, State, TripKey, OutingDate FROM Booking ORDER BY OutingDate ASC"];
    
         while([results next])
         {
             Booking *booking = [[Booking alloc] init];
             NSString *kayakName = [results stringForColumnIndex:0];
             booking.KayakName = [NSString stringWithUTF8String:[kayakName cStringUsingEncoding:NSUTF8StringEncoding]];
        
             booking.Type = [results intForColumnIndex:1];
             NSString *day = [results stringForColumnIndex:2];
             [NSString stringWithUTF8String:[day cStringUsingEncoding:NSUTF8StringEncoding]];
             booking.Day = day;
             booking.Time = [results stringForColumnIndex:3];
             booking.State = [results intForColumnIndex:4];
             booking.TripKey = [results stringForColumnIndex:5];
             booking.OutingDate = [results stringForColumnIndex:6];
        
             [bookingsArray addObject:booking];
         }
         [results close];
     }];
    return bookingsArray;
}

-(NSArray *) getBookingsToCancel
{
    NSMutableArray *arr = [[NSMutableArray alloc]init];
    [_queue inDatabase:^(FMDatabase *db)
     {
        FMResultSet *results = [db executeQuery:@"SELECT TripKey FROM Booking WHERE State = 1"];
         
        while([results next])
        {
            NSString *tripKey = [results stringForColumnIndex:0];
            [arr addObject:tripKey];
        }
        [results close];
     }];
    return arr;
}

-(NSMutableArray *) getForecasts
{
    NSMutableArray *forecastArray = [[NSMutableArray alloc] init];
    [_queue inDatabase:^(FMDatabase *db)
     {
         FMResultSet *results = [db executeQuery:@"SELECT Day,Hour,TempC,WaterTempC,WaveH,Weather,SwellSecs,WindDir,WindF FROM Forecast"];
    
         while([results next])
         {
             Forecast *forecast = [[Forecast alloc] init];
             forecast.Day = [results stringForColumnIndex:0];
             forecast.Hour = [results stringForColumnIndex:1];
             forecast.TempC = [results stringForColumnIndex:2];
             forecast.WaterTempC = [results stringForColumnIndex:3];
             forecast.WaveH = [results stringForColumnIndex:4];
             forecast.Weather = [results stringForColumnIndex:5];
             forecast.SwellSecs = [results stringForColumnIndex:6];
             forecast.WindDir = [results stringForColumnIndex:7];
             forecast.WindF = [results stringForColumnIndex:8];
        
             [forecastArray addObject:forecast];
         }
         [results close];
     }];
    
     [AppLog Log:@"Got %i forecasts", [forecastArray count]];
    
    return forecastArray;
}

-(NSMutableArray *) getKayaksByType:(int) type
{
    NSMutableArray *kayakArray = [[NSMutableArray alloc] init];
    [_queue inDatabase:^(FMDatabase *db)
     {
         FMResultSet *results = [db executeQuery:@"SELECT Key, Name, Type, Weight FROM Kayak WHERE Type = ? ORDER BY Weight DESC",[NSString stringWithFormat:@"%i", type]];
    
        while([results next])
        {
            BoatPref *boat = [[BoatPref alloc] init];
            boat.key = [results stringForColumnIndex:0];
            NSString *kayakName = [results stringForColumnIndex:1];
            boat.name = [NSString stringWithUTF8String:[kayakName cStringUsingEncoding:NSUTF8StringEncoding]];
            boat.type = [results intForColumnIndex:2];
            boat.priority = [results intForColumnIndex:3];
            
            [kayakArray addObject:boat];
        }
        [results close];
     }];
    
    [AppLog Log:@"Got %i kayaks", [kayakArray count]];
    
    return kayakArray;
}


-(NSMutableArray *) getAllKayaks
{
    NSMutableArray *kayakArray = [[NSMutableArray alloc] init];
    [_queue inDatabase:^(FMDatabase *db)
     {
        FMResultSet *results = [db executeQuery:@"SELECT Key, Name, Type, Weight FROM Kayak ORDER BY Weight DESC"];
        
        while([results next])
        {
            BoatPref *boat = [[BoatPref alloc] init];
            boat.key = [results stringForColumnIndex:0];
            NSString *kayakName = [results stringForColumnIndex:1];
            boat.name = [NSString stringWithUTF8String:[kayakName cStringUsingEncoding:NSUTF8StringEncoding]];
            boat.type = [results intForColumnIndex:2];
            boat.priority = [results intForColumnIndex:3];
            
            [kayakArray addObject:boat];
        }
        [results close];
     }];
    
    [AppLog Log:@"%i kayaks were found", [kayakArray count]];
    
    return kayakArray;
}

-(NSMutableArray *) getTimes
{
    NSMutableArray *timeArray = [[NSMutableArray alloc] init];
    
    [_queue inDatabase:^(FMDatabase *db)
     {
        FMResultSet *results = [db executeQuery:@"SELECT DayOfWeek, Time, Type FROM Time ORDER BY DayOfWeek"];
    
        NSMutableDictionary *dayListing = [NSMutableDictionary dictionary];
        NSString *dayOfWeek;
        while([results next])
        {
            dayOfWeek = [results stringForColumnIndex:0];
            DayPref *day = [dayListing objectForKey: dayOfWeek];
            if (day == nil)
                day = [[DayPref alloc]initWithParams:dayOfWeek];
            int time = [results intForColumnIndex:1];
            int type = [results intForColumnIndex:2];
            
            switch (time) {
                case 0:
                    day.Early = type;
                    break;
                case 1:
                    day.Mid = type;
                    break;
                case 2:
                    day.Late = type;
                    break;
                default:
                    break;
            }
            
            [dayListing setObject: day forKey:dayOfWeek];
        }
        [results close];
        
        for (NSString* key in dayListing)
        {
             DayPref* day = [dayListing objectForKey:key];
             [timeArray addObject:day];
        }
         
     }];
    return timeArray;
}


-(DayPref*) getDayOfWeek: (NSString *) dayOfWeek
{
    NSMutableDictionary *dayListing = [NSMutableDictionary dictionary];
    [_queue inDatabase:^(FMDatabase *db)
     {
        FMResultSet *results = [db executeQuery:@"SELECT DayOfWeek, Time, Type FROM Time WHERE DayOfWeek = ?"
                            ,[NSString stringWithFormat:@"%@", dayOfWeek]];
        while([results next])
        {
            NSString *dayOfWeek = [results stringForColumnIndex:0];
            DayPref *day = [dayListing objectForKey: dayOfWeek];
            if (day == nil)
                day = [[DayPref alloc]initWithParams:dayOfWeek];
            int time = [results intForColumnIndex:1];
            int type = [results intForColumnIndex:2];
            
            switch (time) {
                case 0:
                    day.Early = type;
                    break;
                case 1:
                    day.Mid = type;
                    break;
                case 2:
                    day.Late = type;
                    break;
                default:
                    break;
            }
            [dayListing setObject: day forKey:dayOfWeek];
        }
        [results close];
     }];
    
    return [dayListing objectForKey:dayOfWeek];
}

-(void) extractUserSettingsToDb:(PreferenceResponse*) responseObj
{
    int state = responseObj.IsFrozen ? 1 : 0;
    [_queue inDatabase:^(FMDatabase *db)
     {
        [db executeUpdate:@"UPDATE User SET State = ?, Reminder = ?",
         [NSString stringWithFormat:@"%i", state],
         [NSString stringWithFormat:@"%i", responseObj.Reminder], nil];
     }];
}

-(void) updateUserState:(int) state
{
    [_queue inDatabase:^(FMDatabase *db)
     {
        [db executeUpdate:@"UPDATE User SET State = ?",
         [NSString stringWithFormat:@"%i", state], nil];
     }];
    currentUser.state = state;
    
    isDirty = TRUE;
}

-(NSString*)getDeviceToken
{
    __block NSString* token = @"";
    
    [_queue inDatabase:^(FMDatabase *db)
     {
         FMResultSet *results = [db executeQuery:@"SELECT token FROM Device"];
         while([results next])
         {
             token  = [results stringForColumnIndex:0];
             break;
         }
         [results close];
     }];
    
    return token;
}

-(BOOL) updateDeviceToken:(NSString* ) deviceToken
{
    [_queue inTransaction:^(FMDatabase *db, BOOL *rollback)
    {
        [db executeUpdate:@"DELETE FROM Device"];
        [db executeUpdate:@"INSERT OR REPLACE INTO Device(Token) VALUES (?)",
         [NSString stringWithFormat:@"%@", deviceToken], nil];
     }];
    isDirty = true;
    return TRUE;
}

-(BOOL) updateReminder:(int) reminderMins
{
    [_queue inDatabase:^(FMDatabase *db)
     {
         [db executeUpdate:@"UPDATE User SET Reminder = ?",
          [NSString stringWithFormat:@"%i", reminderMins], nil];
     }];
    currentUser.reminder = reminderMins;
    return TRUE;
}

-(BOOL) updateBookingState: (NSString *) key withState: (int) state
{
    [_queue inDatabase:^(FMDatabase *db)
    {
        [db executeUpdate:@"UPDATE Booking SET State = ? WHERE TripKey = ?",
         [NSString stringWithFormat:@"%i", state],
         [NSString stringWithFormat:@"%@", key], nil];
     }];
    isDirty = TRUE;
    return TRUE;
}

-(BOOL) updateKayakPriority: (NSString *) key withPriority: (int) priority
{
    [_queue inDatabase:^(FMDatabase *db)
     {
         [db executeUpdate:@"UPDATE Kayak SET Weight = ? WHERE Key = ?",
          [NSString stringWithFormat:@"%i", priority],
          [NSString stringWithFormat:@"%@", key], nil];
     }];
    
    isDirty = TRUE;
    
    return TRUE;
}

-(BOOL) updateTimeWithParams: (NSString *) dayOfWeek withTime: (int) time withType: (int) type
{
    [_queue inDatabase:^(FMDatabase *db)
     {
        [db executeUpdate:@"INSERT OR REPLACE INTO Time(DayOfWeek, Time, Type) VALUES (?,?,?)",
         [NSString stringWithFormat:@"%@", dayOfWeek],
         [NSString stringWithFormat:@"%i", time],
         [NSString stringWithFormat:@"%i", type], nil];
     }];
    
    isDirty = TRUE;
    return TRUE;
}

-(Set *) getLocalPreferencesFromDb
{
    Set *set = [[Set alloc]init];
    
    NSMutableArray *timeArray = [[NSMutableArray alloc] init];
    for (DayPref* d in self.getTimes)
    {
        if (d.Early > 0){
            [timeArray addObject:[[LightTimePref alloc]initWithParams:d.name withTime:0 withType:d.Early]];
        }
        if (d.Mid > 0){
            [timeArray addObject:[[LightTimePref alloc]initWithParams:d.name withTime:1 withType:d.Mid]];
        }
        if (d.Late > 0)
        {
            [timeArray addObject:[[LightTimePref alloc]initWithParams:d.name withTime:2 withType:d.Late]];
        }
    }
    set.TimePrefs = timeArray;
    NSMutableArray *kayakArray = [[NSMutableArray alloc] init];
    for (BoatPref* k in self.getAllKayaks)
    {
        if (k.priority == 0)
            continue;
        
        [kayakArray addObject:[[LightKayakPref alloc] initWithParams:k.key withName:k.name withType:k.type withWeight:k.priority]];
    }
    set.KayakPrefs = kayakArray;
    return set;
}

-(int) getReminder
{
    __block int reminder = 0;
    [_queue inDatabase:^(FMDatabase *db)
     {
         FMResultSet *results = [db executeQuery:@"SELECT Reminder FROM User WHERE Reminder IS NOT NULL"];
        while([results next])
        {
            reminder = [results intForColumnIndex:0];
        }
         [results close];
     }];
    return reminder;
}

@end
