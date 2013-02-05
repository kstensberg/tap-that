#!/bin/bash

#================================================================#
# Copyright (c) 2010-2011 Zipline Games, Inc.
# All Rights Reserved.
# http://getmoai.com
#================================================================#

#----------------------------------------------------------------#
# application and project names
#----------------------------------------------------------------#

	project_name="TapThat"
	app_name="Tap That"

#----------------------------------------------------------------#
# version numbers
#----------------------------------------------------------------#

	version_code="1"
	version_name="1.0"

#----------------------------------------------------------------#
# space-delimited list of libraries (moai-supported) required 
# (this list is created by make-host.sh using command-line 
# information)
# available: facebook, tapjoy, crittercism, google-push, 
#            google-billing, miscellaneous (required by google-billing)
#----------------------------------------------------------------#

	requires=( "miscellaneous" "adcolony" "google-billing" "chartboost" "crittercism" "facebook" "google-push" "tapjoy" )
		
#----------------------------------------------------------------#
# list of icon files
#----------------------------------------------------------------#

	icon_ldpi="res/icon-ldpi.png"
	icon_mdpi="res/icon-mdpi.png"
	icon_hdpi="res/icon-hdpi.png"
	icon_xhdpi="res/icon-xhdpi.png"

#----------------------------------------------------------------#
# space-delimited list of source lua directories to add to the 
# application bundle and corresponding destination directories in 
# the assets directory of the bundle
#----------------------------------------------------------------#

        src_dirs=( "../../src/" "../../media")
        dest_dirs=(     "src" "media")


#----------------------------------------------------------------#
# working directory in the assets directory of the application 
# bundle and a space-delimited list of lua files thereunder to run 
# when the application starts
#----------------------------------------------------------------#

	working_dir=""
	run=( "src/platforms/mobile.lua" "src/main.lua" )
