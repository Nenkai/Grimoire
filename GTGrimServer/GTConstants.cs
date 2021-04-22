using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer
{
    public class GTConstants
    {
        public const string PDIUserAgent = "pdistd-http library";
        public const string GT6ServiceID_EU = "EP9001-BCES01893_00";
    }

    public enum PFSType
    {
        // Times in seconds - Add 978307200 (from 2001-01-01)

        GT5P_EU_SPEC1_2 = 226279597,    // GT5P-UK-TOTTORI-562314254
        GT5P_US_SPEC3 = 227901480,      // GT5P-US-SONORA-550937027
        GT5P_EU_SPEC3_DIGITAL = 248285681, // GT5P-UK-TOTTORI-562314254

        GT5_TT_EU = 282245958,          // ACADEMY-UK-PISCINAS-77828733

        GT5_KIOSK_DEMO_EU = 300811023,  // GT5E-ARABIAN-312107402

        GT5_JP_V2_11 = 381768656,       // GT5-JP-SAHARA-568201135
        GT5_UK_V2_11 = 381769565,       // GT5-UK-KALAHARI-37863889
        GT5_US_V2_11 = 381769680,       // GT5-US-PATAGONIAN-22798263
        GT5_TW_V2_11 = 381769750,       // GT5-TW-TAKLAMAKAN-63706075

        GTA_2013 = 392922939,           // ACADEMY-CHOLISTAN-901501638

        GT6_GAMESCOM_DEMO = 398721401,  // Build: 2013/08/20 19:56:41

        GT6_V1_00 = 405868366,          // GT6-PISCINAS-323419048
        GT6_V1_02 = 408913268,
        GT6_V1_03 = 410907001,
        GT6_V1_04 = 412168771,
        GT6_V1_05 = 415371169,
        GT6_V1_06 = 417662393,
        GT6_V1_07 = 421135810,
        GT6_V1_08 = 422376318,
        GT6_V1_09 = 424224343,
        GT6_V1_10 = 427224741,
        GT6_V1_11 = 428497826,
        GT6_V1_12 = 431469971,
        GT6_V1_13 = 433078491,
        GT6_V1_14 = 437482543,
        GT6_V1_15 = 439831308,
        GT6_V1_16 = 445620800,
        GT6_V1_17 = 447401670,
        GT6_V1_18 = 449758481,
        GT6_V1_19 = 453817670,
        GT6_V1_20 = 456067192,
        GT6_V1_21 = 464012109,
        GT6_V1_22 = 469128998,
    }

    public enum GT5TrophyType
    {
        PLATINUM,
        ALL_GOLD,
        A_SPEC_LEVEL25,
        B_SPEC_LEVEL25,
        ENDING,
        ENDING2,
        RACE_EVENT_ENDURANCE,
        RACE_EVENT_EXTREME,
        RACE_EVENT_EXPERT,
        RACE_EVENT_PROFESSIONAL,
        RACE_EVENT_AMATEUR,
        RACE_EVENT_BEGINNER,
        LICENSE_S_CLEAR,
        LICENSE_IA_CLEAR,
        LICENSE_IB_CLEAR,
        LICENSE_IC_CLEAR,
        LICENSE_A_CLEAR,
        LICENSE_B_CLEAR,
        SPECIAL_EVENT_VETTEL_CLEAR,
        SPECIAL_EVENT_LOEB_CLEAR,
        SPECIAL_EVENT_GRAND_TOUR_CLEAR,
        SPECIAL_EVENT_RALLY_CLEAR,
        SPECIAL_EVENT_AMG_CLEAR,
        SPECIAL_EVENT_STIG_CLEAR,
        SPECIAL_EVENT_GORDON_CLEAR,
        SPECIAL_EVENT_KART_CLEAR,
        FIRST_WIN,
        FIRST_WIN_B_SPEC,
        UPLOAD_PHOTO,
        UPLOAD_COURSE,
        TUNING_MANIAC,
        MODIFYING_CAR,
        DRIFT_BEGINNER,
        USE_DATA_LOGGER,
        CUSTOM_BGM,
        MILEAGE,
        COLLECTOR,
        MILLIONAIRE,
        OBTAIN_BEFORE50S_CAR,
        OBTAIN_MEMORIAL_CARS,
        OBTAIN_LOVED_CAR,
        OBTAIN_EXPENSIVE_CAR,
        OBTAIN_SUPER_EXPENSIVE_CAR,
        OVER_300KMH,
        OVER_400KMH,
        BY_A_NARROW_MARGIN,
        BATTLE_RIVAL4WD,
        BATTLE_1967,
        GT_R_LAPTIME,
        STABLE_LAPTIME,
        B_SPEC_TROPHY,
        B_SPEC_TROPHY_VETERAN_DRIVER,
        COLLECT_256_PAINT_COLOR,
        PENNILESS,
        DROP_PARTS,
        OVERTURNED,
        TAKE_A_PHOTO_OF_HONDA_AT_HONDA,
        MAN_HUNTER,
        WOMAN_PORTRAIT
    }

    public enum GameType
    {
        Unknown,
        GT5,
        GT6
    }
}
