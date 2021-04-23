using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GTGrimServer.Helpers
{
    public class LogHelper
    {
        public static string[] Humanify(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.Length <= 2)
                    continue;

                string code = arg[0..2];
                if (GT6Codes.TryGetValue(code, out string humanStr))
                    args[i] = humanStr + arg[2..];
            }

            return args;
        }

        public static Dictionary<string, string> GT6Codes = new()
        {
            // community
            { "EC", "create" },
            { "ED", "delete" },
            { "EJ", "join" },
            { "EL", "leave" },

            // race
            { "ET", "best_time_str" },
            { "ER", "repl_time_str" },
            { "EB", "board_id" },
            { "EU", "user_id" },

            // PUTLOG::EVENT
            { "GA", "type" }, // DS - disqalify, UP - update, GL - update license, GM - update mission
            { "GK", "event_id" },
            { "GU", "user" },
            { "GX", "pos_x" },
            { "GZ", "pos_y" },
            { "GR", "result" },
            { "GT", "time" },
            { "GD", "disqualify" },
            { "GM", "game_mode"},

            // PUTLOG::RACE_PRESENT
            { "IU", "user" },
            { "IT", "type_id" },
            { "IC", "category_id" },
            { "I1", "argument1" },
            { "IG", "argument4" },
            { "IF", "f_name" },

            { "JU", "user" },
            { "JA", "type" },
            { "JB", "club_id" },
            { "JC", "club_event_id" },
            { "JE", "tag_id_list_csv" },

            // PUTLOG::ITEM
            { "JH", "head_count" },
            //{ "JB", "body_count" },
            { "JS", "set_count" },

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

            // PUTLOG::VGT
            { "LU", "user" },
            { "LA", "car_label" },
            { "LB", "type" },
            { "LC", "color" },

            // boot
            { "MB", "load_success" },
            { "MU", "user" },

            //{ "NC", "car_label" },
            { "NN", "driver_name" },
            { "NO", "unk" },
            //{ "NN", "score" },

            // PUTLOG::LOGINBONUS
            { "NB", "bonus_ratio" },

            { "NG", "Unk" },
            { "NU", "user" },
            { "NC", "bank_book-diff" },

            // event_setting
            { "OA", "state" }, // PR = Prepare Race, FL = Finalize, ER = Initialize Room Member Info
            { "OB", "server_count" },
            { "OC", "probed_bandwidth1" },
            { "OD", "probed_bandwidth2" },
            { "OM", "whole_room_member_count" },
            { "ON", "member_num" },
            { "OS", "available_server_count" },
            { "OU", "user" },
            { "OR", "available_room_count" },
            { "OX", "room_num" }, // OpenLobbyPublic|QuickMatchStart|ClubLobby|ClubEvent
            { "OY", "room_member_num" }, // OpenLobbyPublic|QuickMatchStart|ClubLobby|ClubEvent
            { "OW", "is_device_wireless" },

            // race_online_room
            { "PR", "region_name" },
            { "PO", "original" },
            { "PF", "partialReplaced" },

            { "PV", "cur_ver" },
            { "PE", "elapsed" },
            { "PU", "user" },


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

            // main - general
            { "RA", "race_state" }, // SR - start, FR - finish, RR - result
            { "RU", "user" },
            { "RM", "gameMode" },
            { "RE", "event_id" },
            { "RC", "course_code" },
            { "RV", "camera" },
            //{ "RC", "controller" },
            { "RN", "windowId" },
            { "RO", "order" },

            { "SA", "monitor_layout" },
            { "SF", "layout_settings" },
            { "SU", "user" },
            { "SC", "car_label" },
            { "ST", "course_code" },

            { "TU", "user" },
            { "TA", "game_mode" },
            { "TB", "course" },
            { "TC", "car" },
            { "TD", "rental" },
            { "TE", "mileage" },
            { "TF", "total_time" },
            { "TG", "scenery" },
            { "TH", "course_id" },

            { "UG", "garage_count" },
            { "US", "garage_stockyard_carcount" },
            { "UC", "cash" },
            { "UD", "gameday" },
            { "UF", "total_photo_count" },
            { "UP", "total_pay" },
            { "UE", "event_id" },
            { "UT", "past_second" },
            { "UU", "user" },

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
            { "VZ", "from" },

            // PUTLOG::WEAR
            { "WU", "user" },
            { "WH", "head_code" },
            { "WI", "head_color" },
            { "WB", "body_code" },
            { "WC", "body_color" },

            // PUTLOG::DLC
            { "XU", "user" },
            { "XA", "type" }, // buy/install
            { "XT", "type" },
            { "XR", "remaining_count" },

            // PUTLOG::CREDIT
            { "YU", "user" },
            { "YT", "type" },
            { "YC", "credit0" },
            { "YN", "cash" },
            { "YA", "args" },

            // withdraw
            { "ZU", "user" },
            { "ZA", "arg1" },
            { "ZB", "arg2" },
            { "ZC", "credit0" },
            { "ZD", "arg3" },
            { "ZN", "cash" },
            { "ZT", "type" },
        };

        public static Dictionary<string, string> RoomConfigTextCodes = new()
        {
            { "GM", "room_game_mode" },
            { "RS", "scope" },
            { "RP", "room_policy" },
            { "QT", "quality_control_template_type" },
            { "TO", "topology" },
            { "VC", "voice_chat" },
            { "MS", "room_max" },
            { "CC", "course_code" },
            { "CI", "generated_course_id" },
            { "RL", "race_limit_laps" },
            { "RW", "weather_percent" },
            { "WA", "weather_accel10" },
            { "CT", "course_time" },
            { "WT", "initial_retention10" },
            { "TP", "time_progress_speed" },
            { "AS", "attack_seperate_type" },
            { "QP", "qualifier_period" },
            { "AL", "alarm_time_value" },
            { "PC", "pit_constraint" },
            { "ST", "start_type" },
            { "NT", "need_tire_change" },
            { "GS", "grid_sort_type" },
            { "TF", "time_to_finish" },
            { "BO", "boost_level" },
            { "VD", "enable_damage" },
            { "PE", "penalty_level" },
            { "BD", "behavior_damage_type" },
            { "CO", "consume_tire" },
            { "SS", "behavior_slip_stream_type" },
            { "LM", "low_mu_type" },
            { "CS", "car_select_method" },
            { "OM", "one_make_model" },
            { "CF", "car_filter_type" },
            { "MC", "country" },
            { "MM", "tuners" },
            { "TA", "car_tag" },
            { "PP", "limit_pp" },
            { "PS", "limit_power" },
            { "NW", "need_weight" },
            { "DT", "need_drivetrain" },
            { "LT", "limit_tire" },
            { "NI", "nitro" },
            { "TU", "tuning" },
            { "SR", "simulation" },
            { "AC", "active_steering" },
            { "SM", "asm" },
            { "DL", "driving_line" },
            { "TC", "tcs" },
            { "AB", "abs" },
        };
    }
}
