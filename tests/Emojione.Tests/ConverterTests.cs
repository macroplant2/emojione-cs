using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emojione.Tests {
    [TestClass]
    public class ConverterTests {

  [TestMethod]
        public void AsciiToUnicode() {
            // single smiley
            string text = ":D";
            string expected = "😃";
            string actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // single smiley with incorrect case (shouldn't convert)
            text = ":d";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // multiple smileys
            text = ";) :p :*";
            expected = "😉 😛 😘";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to start a sentence
            text = @":\ is our confused smiley.";
            expected = "😕 is our confused smiley.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence
            text = "Our smiley to represent joy is :')";
            expected = "Our smiley to represent joy is 😂";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence with puncuation
            text = "The reverse to the joy smiley is the cry smiley :'(.";
            expected = "The reverse to the joy smiley is the cry smiley 😢.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence with preceeding punctuation
            text = @"This is the ""flushed"" smiley: :$.";
            expected = @"This is the ""flushed"" smiley: 😳.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley inside of an IMG tag (shouldn't convert anything inside of the tag)
            text = @"Smile <img class=""e0"" src=""/eo/img/1F604.svg"" alt="":)"" /> because it's going to be a good day.";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley inside of OBJECT tag  (shouldn't convert anything inside of the tag)
            text = @"Smile <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1F604.svg"" type=""image/svg+xml"" standby="":)"">:)</object> because it's going to be a good day.";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // typical username password fail  (shouldn't convert the user:pass, but should convert the last :P)
            text = @"Please log-in with user:pass as your credentials :P.";
            expected = @"Please log-in with user:pass as your credentials 😛.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shouldn't replace an ascii smiley in a URL (shouldn't replace :/)
            text = @"Check out http://www.emojione.com";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortnameToUnicode() {
            // single shortname
            string text = ":snail:";
            string expected = "🐌";
            string actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname mid sentence
            text = "The :snail: is Emoji One's official mascot.";
            expected = "The 🐌 is Emoji One's official mascot.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at start of sentence
            text = ":snail: mail.";
            expected = "🐌 mail.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at start of sentence with apostrophe
            text = ":snail:'s are cool!";
            expected = "🐌's are cool!";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence
            text = "Emoji One's official mascot is :snail:.";
            expected = "Emoji One's official mascot is 🐌.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence with alternate puncuation
            text = "Emoji One's official mascot is :snail:!";
            expected = "Emoji One's official mascot is 🐌!";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence with preceeding colon
            text = "Emoji One's official mascot: :snail:";
            expected = "Emoji One's official mascot: 🐌";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname inside of IMG tag
            text = @"The <img class=""eo"" src=""/img/eo/1F40C.svg"" alt="":snail:"" /> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname inside of OBJECT tag
            text = @"The <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1F40C.svg"" type=""image/svg+xml"" standby="":snail:"">:snail:</object> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname to unicode with code pairs
            text = ":nine:";
            expected = "9⃣";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnicodeToShortname() {
            // single unicode character conversion
            string text = "🐌";
            string expected = ":snail:";
            string actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = "👽 is not :alien: and 저 is not 👽 or 👽";
            expected = ":alien: is not :alien: and 저 is not :alien: or :alien:";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = "💃\n💃";
            expected = ":dancer:\n:dancer:";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // all emoji
            //text = "#️⃣#⃣0️⃣0⃣1️⃣1⃣2️⃣2⃣3️⃣3⃣4️⃣4⃣5️⃣5⃣6️⃣6⃣7️⃣7⃣8️⃣8⃣9️⃣9⃣©®‼️‼⁉️⁉™ℹ️ℹ↔️↔↕️↕↖️↖↗️↗↘️↘↙️↙↩️↩↪️↪⌚️⌚⌛️⌛⏩⏪⏫⏬⏰⏳Ⓜ️Ⓜ▪️▪▫️▫▶️▶◀️◀◻️◻◼️◼◽️◽◾️◾☀️☀☁️☁☎️☎☑️☑☔️☔☕️☕☝️☝☺️☺♈️♈♉️♉♊️♊♋️♋♌️♌♍️♍♎️♎♏️♏♐️♐♑️♑♒️♒♓️♓♠️♠♣️♣♥️♥♦️♦♨️♨♻️♻♿️♿⚓️⚓⚠️⚠⚡️⚡⚪️⚪⚫️⚫⚽️⚽⚾️⚾⛄️⛄⛅️⛅⛎⛔️⛔⛪️⛪⛲️⛲⛳️⛳⛵️⛵⛺️⛺⛽️⛽✂️✂✅✈️✈✉️✉✊✋✌️✌✏️✏✒️✒✔️✔✖️✖✨✳️✳✴️✴❄️❄❇️❇❌❎❓❔❕❗️❗❤️❤➕➖➗➡️➡➰⤴️⤴⤵️⤵⬅️⬅⬆️⬆⬇️⬇⬛️⬛⬜️⬜⭐️⭐⭕️⭕〰〽️〽㊗️㊗㊙️㊙🀄️🀄🃏🅰🅱🅾🅿️🅿🆎🆑🆒🆓🆔🆕🆖🆗🆘🆙🆚🇨🇳🇩🇪🇪🇸🇫🇷🇬🇧🇮🇹🇯🇵🇰🇷🇺🇸🇷🇺🈁🈂🈚️🈚🈯️🈯🈲🈳🈴🈵🈶🈷🈸🈹🈺🉐🉑🌀🌁🌂🌃🌄🌅🌆🌇🌈🌉🌊🌋🌌🌏🌑🌓🌔🌕🌙🌛🌟🌠🌰🌱🌴🌵🌷🌸🌹🌺🌻🌼🌽🌾🌿🍀🍁🍂🍃🍄🍅🍆🍇🍈🍉🍊🍌🍍🍎🍏🍑🍒🍓🍔🍕🍖🍗🍘🍙🍚🍛🍜🍝🍞🍟🍠🍡🍢🍣🍤🍥🍦🍧🍨🍩🍪🍫🍬🍭🍮🍯🍰🍱🍲🍳🍴🍵🍶🍷🍸🍹🍺🍻🎀🎁🎂🎃🎄🎅🎆🎇🎈🎉🎊🎋🎌🎍🎎🎏🎐🎑🎒🎓🎠🎡🎢🎣🎤🎥🎦🎧🎨🎩🎪🎫🎬🎭🎮🎯🎰🎱🎲🎳🎴🎵🎶🎷🎸🎹🎺🎻🎼🎽🎾🎿🏀🏁🏂🏃🏄🏆🏈🏊🏠🏡🏢🏣🏥🏦🏧🏨🏩🏪🏫🏬🏭🏮🏯🏰🐌🐍🐎🐑🐒🐔🐗🐘🐙🐚🐛🐜🐝🐞🐟🐠🐡🐢🐣🐤🐥🐦🐧🐨🐩🐫🐬🐭🐮🐯🐰🐱🐲🐳🐴🐵🐶🐷🐸🐹🐺🐻🐼🐽🐾👀👂👃👄👅👆👇👈👉👊👋👌👍👎👏👐👑👒👓👔👕👖👗👘👙👚👛👜👝👞👟👠👡👢👣👤👦👧👨👩👪👫👮👯👰👱👲👳👴👵👶👷👸👹👺👻👼👽👾👿💀📇💁💂💃💄💅📒💆📓💇📔💈📕💉📖💊📗💋📘💌📙💍📚💎📛💏📜💐📝💑📞💒📟📠💓📡📢💔📣📤💕📥📦💖📧📨💗📩📪💘📫📮💙📰📱💚📲📳💛📴📶💜📷📹💝📺📻💞📼🔃💟🔊🔋💠🔌🔍💡🔎🔏💢🔐🔑💣🔒🔓💤🔔🔖💥🔗🔘💦🔙🔚💧🔛🔜💨🔝🔞💩🔟💪🔠🔡💫🔢🔣💬🔤🔥💮🔦🔧💯🔨🔩💰🔪🔫💱🔮💲🔯💳🔰🔱💴🔲🔳💵🔴🔵💸🔶🔷💹🔸🔹💺🔺🔻💻🔼💼🔽🕐💽🕑💾🕒💿🕓📀🕔🕕📁🕖🕗📂🕘🕙📃🕚🕛📄🗻🗼📅🗽🗾📆🗿😁😂😃📈😄😅📉😆😉📊😊😋📋😌😍📌😏😒📍😓😔📎😖😘📏😚😜📐😝😞📑😠😡😢😣😤😥😨😩😪😫😭😰😱😲😳😵😷😸😹😺😻😼😽😾😿🙀🙅🙆🙇🙈🙉🙊🙋🙌🙍🙎🙏🚀🚃🚄🚅🚇🚉🚌🚏🚑🚒🚓🚕🚗🚙🚚🚢🚤🚥🚧🚨🚩🚪🚫🚬🚭🚲🚶🚹🚺🚻🚼🚽🚾🛀😀😇😈😎😐😑😕😗😙😛😟😦😧😬😮😯😴😶🚁🚂🚆🚈🚊🚍🚎🚐🚔🚖🚘🚛🚜🚝🚞🚟🚠🚡🚣🚦🚮🚯🚰🚱🚳🚴🚵🚷🚸🚿🛁🛂🛃🛄🛅🌍🌎🌐🌒🌖🌗🌘🌚🌜🌝🌞🌲🌳🍋🍐🍼🏇🏉🏤🐀🐁🐂🐃🐄🐅🐆🐇🐈🐉🐊🐋🐏🐐🐓🐕🐖🐪👥👬👭💭💶💷📬📭📯📵🔀🔁🔂🔄🔅🔆🔇🔉🔕🔬🔭🕜🕝🕞🕟🕠🕡🕢🕣🕤🕥🕦🕧";
            //expected = ":hash::hash::zero::zero::one::one::two::two::three::three::four::four::five::five::six::six::seven::seven::eight::eight::nine::nine::copyright::registered::bangbang::bangbang::interrobang::interrobang::tm::information_source::information_source::left_right_arrow::left_right_arrow::arrow_up_down::arrow_up_down::arrow_upper_left::arrow_upper_left::arrow_upper_right::arrow_upper_right::arrow_lower_right::arrow_lower_right::arrow_lower_left::arrow_lower_left::leftwards_arrow_with_hook::leftwards_arrow_with_hook::arrow_right_hook::arrow_right_hook::watch::watch::hourglass::hourglass::fast_forward::rewind::arrow_double_up::arrow_double_down::alarm_clock::hourglass_flowing_sand::m::m::black_small_square::black_small_square::white_small_square::white_small_square::arrow_forward::arrow_forward::arrow_backward::arrow_backward::white_medium_square::white_medium_square::black_medium_square::black_medium_square::white_medium_small_square::white_medium_small_square::black_medium_small_square::black_medium_small_square::sunny::sunny::cloud::cloud::telephone::telephone::ballot_box_with_check::ballot_box_with_check::umbrella::umbrella::coffee::coffee::point_up::point_up::relaxed::relaxed::aries::aries::taurus::taurus::gemini::gemini::cancer::cancer::leo::leo::virgo::virgo::libra::libra::scorpius::scorpius::sagittarius::sagittarius::capricorn::capricorn::aquarius::aquarius::pisces::pisces::spades::spades::clubs::clubs::hearts::hearts::diamonds::diamonds::hotsprings::hotsprings::recycle::recycle::wheelchair::wheelchair::anchor::anchor::warning::warning::zap::zap::white_circle::white_circle::black_circle::black_circle::soccer::soccer::baseball::baseball::snowman::snowman::partly_sunny::partly_sunny::ophiuchus::no_entry::no_entry::church::church::fountain::fountain::golf::golf::sailboat::sailboat::tent::tent::fuelpump::fuelpump::scissors::scissors::white_check_mark::airplane::airplane::envelope::envelope::fist::raised_hand::v::v::pencil2::pencil2::black_nib::black_nib::heavy_check_mark::heavy_check_mark::heavy_multiplication_x::heavy_multiplication_x::sparkles::eight_spoked_asterisk::eight_spoked_asterisk::eight_pointed_black_star::eight_pointed_black_star::snowflake::snowflake::sparkle::sparkle::x::negative_squared_cross_mark::question::grey_question::grey_exclamation::exclamation::exclamation::heart::heart::heavy_plus_sign::heavy_minus_sign::heavy_division_sign::arrow_right::arrow_right::curly_loop::arrow_heading_up::arrow_heading_up::arrow_heading_down::arrow_heading_down::arrow_left::arrow_left::arrow_up::arrow_up::arrow_down::arrow_down::black_large_square::black_large_square::white_large_square::white_large_square::star::star::o::o::wavy_dash::part_alternation_mark::part_alternation_mark::congratulations::congratulations::secret::secret::mahjong::mahjong::black_joker::a::b::o2::parking::parking::ab::cl::cool::free::id::new::ng::ok::sos::up::vs::cn::de::es::fr::gb::it::jp::kr::us::ru::koko::sa::u7121::u7121::u6307::u6307::u7981::u7a7a::u5408::u6e80::u6709::u6708::u7533::u5272::u55b6::ideograph_advantage::accept::cyclone::foggy::closed_umbrella::night_with_stars::sunrise_over_mountains::sunrise::city_dusk::city_sunset::rainbow::bridge_at_night::ocean::volcano::milky_way::earth_asia::new_moon::first_quarter_moon::waxing_gibbous_moon::full_moon::crescent_moon::first_quarter_moon_with_face::star2::stars::chestnut::seedling::palm_tree::cactus::tulip::cherry_blossom::rose::hibiscus::sunflower::blossom::corn::ear_of_rice::herb::four_leaf_clover::maple_leaf::fallen_leaf::leaves::mushroom::tomato::eggplant::grapes::melon::watermelon::tangerine::banana::pineapple::apple::green_apple::peach::cherries::strawberry::hamburger::pizza::meat_on_bone::poultry_leg::rice_cracker::rice_ball::rice::curry::ramen::spaghetti::bread::fries::sweet_potato::dango::oden::sushi::fried_shrimp::fish_cake::icecream::shaved_ice::ice_cream::doughnut::cookie::chocolate_bar::candy::lollipop::custard::honey_pot::cake::bento::stew::egg::fork_and_knife::tea::sake::wine_glass::cocktail::tropical_drink::beer::beers::ribbon::gift::birthday::jack_o_lantern::christmas_tree::santa::fireworks::sparkler::balloon::tada::confetti_ball::tanabata_tree::crossed_flags::bamboo::dolls::flags::wind_chime::rice_scene::school_satchel::mortar_board::carousel_horse::ferris_wheel::roller_coaster::fishing_pole_and_fish::microphone::movie_camera::cinema::headphones::art::tophat::circus_tent::ticket::clapper::performing_arts::video_game::dart::slot_machine::8ball::game_die::bowling::flower_playing_cards::musical_note::notes::saxophone::guitar::musical_keyboard::trumpet::violin::musical_score::running_shirt_with_sash::tennis::ski::basketball::checkered_flag::snowboarder::runner::surfer::trophy::football::swimmer::house::house_with_garden::office::post_office::hospital::bank::atm::hotel::love_hotel::convenience_store::school::department_store::factory::izakaya_lantern::japanese_castle::european_castle::snail::snake::racehorse::sheep::monkey::chicken::boar::elephant::octopus::shell::bug::ant::bee::beetle::fish::tropical_fish::blowfish::turtle::hatching_chick::baby_chick::hatched_chick::bird::penguin::koala::poodle::camel::dolphin::mouse::cow::tiger::rabbit::cat::dragon_face::whale::horse::monkey_face::dog::pig::frog::hamster::wolf::bear::panda_face::pig_nose::feet::eyes::ear::nose::lips::tongue::point_up_2::point_down::point_left::point_right::punch::wave::ok_hand::thumbsup::thumbsdown::clap::open_hands::crown::womans_hat::eyeglasses::necktie::shirt::jeans::dress::kimono::bikini::womans_clothes::purse::handbag::pouch::mans_shoe::athletic_shoe::high_heel::sandal::boot::footprints::bust_in_silhouette::boy::girl::man::woman::family::couple::cop::dancers::bride_with_veil::person_with_blond_hair::man_with_gua_pi_mao::man_with_turban::older_man::older_woman::baby::construction_worker::princess::japanese_ogre::japanese_goblin::ghost::angel::alien::space_invader::imp::skull::card_index::information_desk_person::guardsman::dancer::lipstick::nail_care::ledger::massage::notebook::haircut::notebook_with_decorative_cover::barber::closed_book::syringe::book::pill::green_book::kiss::blue_book::love_letter::orange_book::ring::books::gem::name_badge::couplekiss::scroll::bouquet::pencil::couple_with_heart::telephone_receiver::wedding::pager::fax::heartbeat::satellite::loudspeaker::broken_heart::mega::outbox_tray::two_hearts::inbox_tray::package::sparkling_heart::e-mail::incoming_envelope::heartpulse::envelope_with_arrow::mailbox_closed::cupid::mailbox::postbox::blue_heart::newspaper::iphone::green_heart::calling::vibration_mode::yellow_heart::mobile_phone_off::signal_strength::purple_heart::camera::video_camera::gift_heart::tv::radio::revolving_hearts::vhs::arrows_clockwise::heart_decoration::loud_sound::battery::diamond_shape_with_a_dot_inside::electric_plug::mag::bulb::mag_right::lock_with_ink_pen::anger::closed_lock_with_key::key::bomb::lock::unlock::zzz::bell::bookmark::boom::link::radio_button::sweat_drops::back::end::droplet::on::soon::dash::top::underage::poop::keycap_ten::muscle::capital_abcd::abcd::dizzy::1234::symbols::speech_balloon::abc::fire::white_flower::flashlight::wrench::100::hammer::nut_and_bolt::moneybag::knife::gun::currency_exchange::crystal_ball::heavy_dollar_sign::six_pointed_star::credit_card::beginner::trident::yen::black_square_button::white_square_button::dollar::red_circle::large_blue_circle::money_with_wings::large_orange_diamond::large_blue_diamond::chart::small_orange_diamond::small_blue_diamond::seat::small_red_triangle::small_red_triangle_down::computer::arrow_up_small::briefcase::arrow_down_small::clock1::minidisc::clock2::floppy_disk::clock3::cd::clock4::dvd::clock5::clock6::file_folder::clock7::clock8::open_file_folder::clock9::clock10::page_with_curl::clock11::clock12::page_facing_up::mount_fuji::tokyo_tower::date::statue_of_liberty::japan::calendar::moyai::grin::joy::smiley::chart_with_upwards_trend::smile::sweat_smile::chart_with_downwards_trend::laughing::wink::bar_chart::blush::yum::clipboard::relieved::heart_eyes::pushpin::smirk::unamused::round_pushpin::sweat::pensive::paperclip::confounded::kissing_heart::straight_ruler::kissing_closed_eyes::stuck_out_tongue_winking_eye::triangular_ruler::stuck_out_tongue_closed_eyes::disappointed::bookmark_tabs::angry::rage::cry::persevere::triumph::disappointed_relieved::fearful::weary::sleepy::tired_face::sob::cold_sweat::scream::astonished::flushed::dizzy_face::mask::smile_cat::joy_cat::smiley_cat::heart_eyes_cat::smirk_cat::kissing_cat::pouting_cat::crying_cat_face::scream_cat::no_good::ok_woman::bow::see_no_evil::hear_no_evil::speak_no_evil::raising_hand::raised_hands::person_frowning::person_with_pouting_face::pray::rocket::railway_car::bullettrain_side::bullettrain_front::metro::station::bus::busstop::ambulance::fire_engine::police_car::taxi::red_car::blue_car::truck::ship::speedboat::traffic_light::construction::rotating_light::triangular_flag_on_post::door::no_entry_sign::smoking::no_smoking::bike::walking::mens::womens::restroom::baby_symbol::toilet::wc::bath::grinning::innocent::smiling_imp::sunglasses::neutral_face::expressionless::confused::kissing::kissing_smiling_eyes::stuck_out_tongue::worried::frowning::anguished::grimacing::open_mouth::hushed::sleeping::no_mouth::helicopter::steam_locomotive::train2::light_rail::tram::oncoming_bus::trolleybus::minibus::oncoming_police_car::oncoming_taxi::oncoming_automobile::articulated_lorry::tractor::monorail::mountain_railway::suspension_railway::mountain_cableway::aerial_tramway::rowboat::vertical_traffic_light::put_litter_in_its_place::do_not_litter::potable_water::non-potable_water::no_bicycles::bicyclist::mountain_bicyclist::no_pedestrians::children_crossing::shower::bathtub::passport_control::customs::baggage_claim::left_luggage::earth_africa::earth_americas::globe_with_meridians::waxing_crescent_moon::waning_gibbous_moon::last_quarter_moon::waning_crescent_moon::new_moon_with_face::last_quarter_moon_with_face::full_moon_with_face::sun_with_face::evergreen_tree::deciduous_tree::lemon::pear::baby_bottle::horse_racing::rugby_football::european_post_office::rat::mouse2::ox::water_buffalo::cow2::tiger2::leopard::rabbit2::cat2::dragon::crocodile::whale2::ram::goat::rooster::dog2::pig2::dromedary_camel::busts_in_silhouette::two_men_holding_hands::two_women_holding_hands::thought_balloon::euro::pound::mailbox_with_mail::mailbox_with_no_mail::postal_horn::no_mobile_phones::twisted_rightwards_arrows::repeat::repeat_one::arrows_counterclockwise::low_brightness::high_brightness::mute::sound::no_bell::microscope::telescope::clock130::clock230::clock330::clock430::clock530::clock630::clock730::clock830::clock930::clock1030::clock1130::clock1230:";
            //actual = EmojiHelper.UnicodeToShortname(text);
            //Assert.AreEqual(expected, actual);

            // single character with surrogate pair
            text = "9⃣";
            expected = ":nine:";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character mid sentence
            text = "The 🐌 is Emoji One's official mascot.";
            expected = "The :snail: is Emoji One's official mascot.";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character mid sentence with a comma
            text = "The 🐌, is Emoji One's official mascot.";
            expected = "The :snail:, is Emoji One's official mascot.";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character at start of sentence
            text = "🐌 mail.";
            expected = ":snail: mail.";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character at start of sentence with apostrophe
            text = "🐌's are cool!";
            expected = ":snail:'s are cool!";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence
            text = "Emoji One's official mascot is 🐌.";
            expected = "Emoji One's official mascot is :snail:.";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence with alternate puncuation
            text = "Emoji One's official mascot is 🐌!";
            expected = "Emoji One's official mascot is :snail:!";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence with preceeding colon
            text = "Emoji One's official mascot: 🐌";
            expected = "Emoji One's official mascot: :snail:";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // character inside of IMG tag
            text = @"The <img class=""eo"" src=""/img/eo/1F40C.svg"" alt=""🐌"" /> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // characters inside of OBJECT tag
            text = @"The <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1F40C.svg"" type=""image/svg+xml"" standby=""🐌"">🐌</object> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);

            // unicode alternate to short
            text = "#️⃣"; // 0023-fe0f-20e3
            expected = ":hash:";
            actual = Emojione.UnicodeToShortname(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnifyUnicode() {
            // single unicode character conversion
            string text = ":snail:";
            string expected = "🐌";
            string actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = "👽 is 👽 and 저 is not 👽 or 👽 also :randomy: is not emoji";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = ":dancer:\n:dancer:";
            expected = "💃\n💃";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // triple emoji string
            text = ":dancer::dancer::alien:";
            expected = "💃💃👽";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToImage() {
            // single character shortname conversion
            string text = ":snail:";
            string expected = @"<img class=""eo"" src=""/img/eo/1F40C.svg"" alt="":snail:"" />";
            string actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // shortname shares a colon
            text = ":invalid:snail:";
            expected = ":invalid:snail:";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // single unicode character conversion
            text = "🐌";
            expected = @"<img class=""eo"" src=""/img/eo/1F40C.svg"" alt="":snail:"" />";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = @"<img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:"" /> is <img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:"" /> and 저 is not <img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:"" /> or <img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:"" /> also :randomy: is not emoji";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnicodeToCodepoint() {
            string unicode = "😀"; // :grinning:
            string expected = "1f600";
            string actual = Emojione.ToCodePoint(unicode);
            Assert.AreEqual(expected, actual);

            expected = "D83D-DE00";
            actual = ShowX4(unicode);
            Assert.AreEqual(expected, actual);

            string codepoint = "1f600";
            expected = "😀";
            actual = Emojione.ToUnicode(codepoint);
            Assert.AreEqual(expected, actual);
            expected = "D83D-DE00";
            actual = ShowX4(actual);
            Assert.AreEqual(expected, actual);

            expected = "\uD83D\uDE00";
            actual = "😀";
            Assert.AreEqual(expected, actual);

            expected = "\\uD83D\\uDE00";
            actual = ToSurrogateString("1f600");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ImageToShortname() {
            string html = @"I am an <i class=""eo eo-1F47D"" title="":alien:"">👽</i>";
            string actual = Emojione.ImageToShortname(html);
            string expected = "I am an :alien:";
            Assert.AreEqual(expected, actual);

            html = @"I am an <i class=""eo eo-alien"">:alien:</i>";
            actual = Emojione.ImageToShortname(html);
            expected = "I am an :alien:";
            Assert.AreEqual(expected, actual);

            html = @"I am an <img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:"" />";
            actual = Emojione.ImageToShortname(html);
            expected = "I am an :alien:";
            Assert.AreEqual(expected, actual);

            html = @"I am an <img class=""eo"" src=""/img/eo/1F47D.svg"" alt="":alien:""/>";
            actual = Emojione.ImageToShortname(html);
            expected = "I am an :alien:";
            Assert.AreEqual(expected, actual);

            html = @"I am an <img class=""eo"" src=""/img/eo/1F47D.svg"" alt=""👽"">";
            actual = Emojione.ImageToShortname(html);
            expected = "I am an :alien:";
            Assert.AreEqual(expected, actual);

            html = @"We are <img class=""eo"" src=""/img/eo/1F468-1F469-1F466-1F466.svg"" alt=""👨👩👦👦"">";
            actual = Emojione.ImageToShortname(html);
            expected = "We are :family_mwbb:";

            html = @"We are <img class=""eo"" src=""/img/eo/1F468-1F469-1F466-1F466.svg"" alt="":family_mwbb:"">";
            actual = Emojione.ImageToShortname(html);
            expected = "We are :family_mwbb:";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CodePointToUnicode() {
            // U+1D161 = MUSICAL SYMBOL SIXTEENTH NOTE 
            int music = 0x1D161; // 119137

            // Convert the UTF32 code point U+1D161 to UTF-16. The UTF-16 equivalent of  
            // U+1D161 is a surrogate pair with hexadecimal values D834 and DD61.
            string s1 = Char.ConvertFromUtf32(music);
            string s2 = ShowX4(s1, music); // D834-DD61 => 1D161

            // Convert the surrogate pair in the string at index position  zero to a code point.
            music = Char.ConvertToUtf32(s1, 0); // 119137
            s2 = ShowX4(s1, music); // D834-DD61 => 1D161

            // Convert the high and low characters in the surrogate pair into a code point.
            music = Char.ConvertToUtf32(s1[0], s1[1]); // 119137
            s2 = ShowX4(s1, music);  // D834-DD61 => 1D161

            string ninestr = "9⃣";
            string expected = "0039-20e3";
            string actual = Emojione.ToCodePoint(ninestr); // 0039-20e3
            Assert.AreEqual(expected, actual);

            var s = actual.Split('-');
            char[] parts = new char[2];
            for (int i = 0; i < s.Length; i++) {
                var part = Convert.ToInt32(s[i], 16); // part[0] = 57, part[1] = 8419
                if (part >= 0x10000 && part <= 0x10FFFF) {
                    var hi = (int)Math.Floor((decimal)(part - 0x10000) / 0x400) + 0xD800;
                    var lo = (int)((part - 0x10000) % 0x400) + 0xDC00;
                    //part = (String.fromCharCode(hi) + String.fromCharCode(lo));
                } else {
                    parts[i] = (char)part;
                }
            }
            var str = new String(parts);
            Assert.AreEqual(ninestr, str);
        }

        private static string ShowX4(string s, int? i = null) {
            string s2 = "";
            for (int x = 0; x < s.Length; x++) {
                s2 += string.Format("{0:X4}{1}", (int)s[x], ((x == s.Length - 1) ? String.Empty : "-"));
            }
            if (i != null) {
                return string.Format("{0} => {1:X}", s2, i);
            } else {
                return s2;
            }
        }

        /// <summary>
        /// Converts a unicode character to surrogate pairs
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        private static string ToSurrogateString(string codepoint) {
            string unicode = Emojione.ToUnicode(codepoint);
            string s2 = "";
            for (int x = 0; x < unicode.Length; x++) {
                s2 += string.Format("\\u{0:X4}", (int)unicode[x]);
            }
            return s2;
        }

    }
}
