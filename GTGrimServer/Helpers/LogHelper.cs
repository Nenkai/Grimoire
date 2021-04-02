using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Helpers
{
    public class LogHelper
    {
        public static Dictionary<string, string> Codes = new()
        {
            // boot
            { "MB", "load_success" },
            { "MU", "user" },

            { "NG", "Unk" },
            { "NU", "user" },
            { "NC", "bank_book-diff" },
            { "UG", "garage_count" },
            { "US", "garage_stockyard_carcount" },
            { "UC", "cash" },
            { "UD", "gameday" },
            { "UF", "total_photo_count" },
            { "UP", "total_pay" },
            { "UE", "event_id" },
            { "UT", "past_second" },
            { "UU", "user" },

            { "SA", "Monitor Layout" },
            { "SF", "Layout Settings" },
            { "SU", "user" },
            { "SC", "car_label" },
            { "ST", "course_code" },

            { "PV", "cur_ver" },
            { "PE", "elapsed" },
            { "PU", "user" },

            // community
            { "EC", "create" },
            { "ED", "delete" },
            { "EJ", "join" },
            { "EL", "leave" },

            { "JU", "user" },
            { "JA", "type" },
            { "JB", "club_id" },
            { "JC", "club_event_id" },
            { "JE", "tag_id_list_csv" },

            // event_setting
            { "OA", "state" }, // PR = Prepare Race
            { "OB", "byAlarm" },
            { "ON", "memberNum" },
            { "OU", "user" },
            { "OR", "roomConfigString" },

            // race
            { "ET", "best_time_str" },
            { "ER", "repl_time_str" },
            { "EB", "board_id" },
            { "EU", "user_id" },

            // race_online_room
            { "PR", "region_name" },
            { "PO", "original" },
            { "PF", "partialReplaced" },

            //{ "NC", "car_label" },
            { "NN", "driver_name" },
            { "NO", "unk" },
            //{ "NN", "score" },

            // main - general
            { "RA", "Race State" }, // SR - start, FR - finish, RR - result
            { "RU", "user" },
            { "RM", "gameMode" },
            { "RE", "event_id" },
            { "RC", "course_code" },
            { "RV", "camera" },
            //{ "RC", "controller" },
            { "RN", "windowId" },
            { "RO", "order" },

            { "TU", "user" },
            { "TA", "game_mode" },
            { "TB", "course" },
            { "TC", "car" },
            { "TD", "rental" },
            { "TE", "mileage" },
            { "TF", "total_time" },
            { "TG", "scenery" },
            { "TH", "course_id" },

            // PUTLOG::WEAR
            { "WU", "user" },
            { "WH", "head_code" },
            { "WI", "head_color" },
            { "WB", "body_code" },
            { "WC", "body_color" },

            // PUTLOG::ITEM
            { "JH", "head_count" },
            //{ "JB", "body_count" },
            { "JS", "set_count" },

            // PUTLOG::RACE_PRESENT
            { "IU", "user" },
            { "IT", "type_id" },
            { "IC", "category_id" },
            { "I1", "argument1" },
            { "IG", "argument4" },
            { "IF", "f_name" },

            // PUTLOG::DLC
            { "XU", "user" },
            { "XA", "type" }, // buy/install
            { "XT", "type" },
            { "XR", "remaining_count" },

            // PUTLOG::EVENT
            { "GA", "type" }, // DS - disqalify, UP - update, GL - update license, GM - update mission
            { "GK", "event_id" },
            { "GU", "user" },
            { "GX", "pos_x" },
            { "GZ", "pos_y" },
            { "GR", "result" },
            { "GT", "time" },
            { "GD", "disqualify" },

            // PUTLOG::CREDIT
            { "YU", "user" },
            { "YT", "type" },
            { "YC", "credit0" },
            { "YN", "cash" },
            { "YA", "args" },

            // withdraw
            { "ZU", "user" },
            { "ZT", "type" },
            //{ "ZC", "credit0" },
            { "ZN", "cash" },
            { "ZA", "arg1" },
            { "ZB", "arg2" },
            { "ZC", "arg3" },

            // PUTLOG::LOGINBONUS
            { "NB", "bonus_ratio" },

            // PUTLOG::CURRENT
            { "VU", "user" },
            { "VA", "carrer_mode_license" },
            { "VB", "offline_trophy_count" },
            { "VC", "license_trophy_count" },
            { "VD", "stars" },
            { "VE", "car_count" },
            { "VF", "total_aspec_race_complete" },
            { "VG", "total_aspec_win" },
            { "VH", "cash" },
            { "VI", "total_prize" },
            { "VJ", "total_tuning_cost" },
            { "VK", "total_aspec_running_km" },
            { "VL", "total_aspec_time" },
            { "VM", "total_photo_count" },
            { "VN", "paint_count" },
            { "VO", "racing_gear_count" },
            { "VP", "dlc_info" },
            { "VQ", "total_gas_consumption" },
            { "VR", "total_all_gas_consumption" },

            // PUTLOG::VGT
            { "LU", "user" },
            { "LA", "car_label" },
            { "LB", "type" },
            { "LC", "color" },

            // PUTLOG::TIMERALLY
            { "KU", "user" },
            { "KA", "event_id" },
            { "KB", "complete" },
            { "KC", "score" },
            { "KD", "remain_second" },
            { "KE", "overtake" },
            { "KF", "combo" },
            { "KG", "perfect" },
            { "KH", "current_section" },

            // User Environment
            { "QA", "resolution" },
            { "QM", "macaddr" },
            { "QW", "wireless" },
            { "QU", "account" },
            { "QN", "nat" },
            { "QR", "region" },
            { "QL", "lang" },
            { "Q3", "is3DTV" },
            { "QF", "freeDiskSpace" },
        };

        public static string[] Humanify(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.Length <= 2)
                    continue;

                string code = arg[0..2];
                if (Codes.TryGetValue(code, out string humanStr))
                    args[i] = humanStr + arg[2..];                
            }

            return args;
        }
    }
}
