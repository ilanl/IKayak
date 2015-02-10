//
//  Logger.m
//  KayApp
//
//  Created by Ilan Levy on 10/15/13.
//  Copyright (c) 2013 Ilan Levy. All rights reserved.
//

#import "AppLog.h"

@implementation AppLog
NSDateFormatter *formatter;
NSString *filePath;
@synthesize level;
static AppLog *instance = NULL;
BOOL enableLogging = FALSE;

-(id) init
{
    self = [super init];
    
    if (self) {
        
        NSString *documentsDirectory = [NSHomeDirectory() stringByAppendingPathComponent:@"Documents"];
        filePath = [documentsDirectory stringByAppendingPathComponent:@"app.log"];
        enableLogging = [[[NSBundle mainBundle] objectForInfoDictionaryKey:@"enable_log"]  isEqual: @"1"];
        
        if (!enableLogging)
        NSLog(@"Logger is disabled");
    }
    
    return self;
}

- (void)Log3:(NSString* )format arguments:(va_list)argList NS_FORMAT_FUNCTION(1,0)
{
    
    NSString* str = [NSString stringWithFormat:format,argList];
    NSLog(format,argList);
    
    NSFileHandle* fh = [NSFileHandle fileHandleForWritingAtPath:filePath];
    if ( !fh ) {
        [[NSFileManager defaultManager] createFileAtPath:filePath contents:nil attributes:nil];
        fh = [NSFileHandle fileHandleForWritingAtPath:filePath];
        
    }
    @try {
        [fh seekToEndOfFile];
        [fh writeData:[str dataUsingEncoding:NSUTF8StringEncoding]];
    }
    @catch (NSException * e) {
        NSLog(@"log error: %@", e.description);
    }
    [fh closeFile];

}

+ (void)Log:(NSString *) formatString, ...
{
    
    @synchronized(self) {
        if (instance == NULL) {
            instance = [[self alloc] init];
        }
        
        if (!enableLogging)
            return;
        
        NSString *dateString;
        formatter = [[NSDateFormatter alloc] init];
        [formatter setDateFormat:@"dd-MM-yyyy HH:mm"];
        dateString = [formatter stringFromDate:[NSDate date]];
        
        NSString* contents = nil;
        
        va_list args;
        va_start(args, formatString);
        contents = [[NSString alloc] initWithFormat:formatString arguments:args];
        va_end(args);
        
        NSLog(@"%@",contents);
        NSFileHandle* fh = [NSFileHandle fileHandleForWritingAtPath:filePath];
        
        if ( !fh ) {
            [[NSFileManager defaultManager] createFileAtPath:filePath contents:nil attributes:nil];
            fh = [NSFileHandle fileHandleForWritingAtPath:filePath];
            
        }
        @try {
            [fh seekToEndOfFile];
            contents = [[NSString alloc] initWithFormat:@"%@ %@", dateString, contents];
            [fh writeData:[[[NSString alloc] initWithFormat:@"%@\n", contents] dataUsingEncoding:NSUTF8StringEncoding]];
        }
        @catch (NSException * e) {
            NSLog(@"log error: %@", e.description);
        }
        [fh closeFile];
    }
}


@end
