include ~/.mk/config.mk
include ~/.mk/msbuild.mk

# msbuild configuration
APP_NAME = M64
APP_SRCDIR = ${SRCDIR}/${APP_NAME}
APP_OUTDIR = ${OUTDIR}/${APP_NAME}

.DEFAULT_GOAL = all

.PHONY: all

all: clean build-app ## Run all targets (default)
