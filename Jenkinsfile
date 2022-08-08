#!/usr/bin/env groovy

@Library(["sharedbuild", "slack-helpers"])_

pipeline {
  agent none;

  parameters {
    booleanParam(name: "RELEASE", defaultValue: false, description: "Release")
    string(name: "RELEASE_NOTES", defaultValue: "", description: "Release Note")
  }

  environment {
    ENVIRONMENT = "dev"
    DEBUG_BUILD = true
  }

  stages {

    stage('Setup'){
      agent {
        label 'MacOS'
      }

      steps {
        loadProperties([propertiesFile: "Jenkins.properties"])
        setBuildNumber()
        logBuildData()
      }
    }

    stage('Build') {
      agent {
        label 'MacOS'
      }

      stages {

        stage("Build CS"){
          steps{
            unsafeEcho("Running on: ${env.NODE_NAME}")

            dir("${env.CS_SOLUTION_DIRECTORY}"){
              sh ( script:"./build.sh" )
            }
          }
        }

        stage('Unit Tests') {
          when{
            expression { return env.TEST_DLLS != null && !env.TEST_DLLS.isEmpty() }
          }
          steps {
            unsafeEcho("Running on: ${env.NODE_NAME}")

            consoleUnitTest (
                    testDlls: [env.TEST_DLLS]
            )
          }
        }
		
		stage("Run Unity Tests") {
            steps {
			  sh ( "echo $PWD" )
			  sh ( "chmod +x ./copy-dlls-to-unity.sh" )
			  sh ( script:"./copy-dlls-to-unity.sh" )
              
			  script {
                def testSettings = [:]
                testSettings.environment = "${env.ENVIRONMENT}"
                testSettings.debugBuild = "${env.DEBUG_BUILD}"
                testSettings.unityVersion = "${env.UNITY_VERSION}"
                testSettings.projectPath = "${env.UNITY_PROJECT_PATH}"
                testSettings.buildNumberPrefix = "${env.BUILD_NUMBER_PREFIX}"
                testSettings.globalBuildId = "${env.BUILD_NUMBER_SUFFIX}"
                testSettings.environmentVariables = getEnvironmentVariables(testSettings)
                testSettings.platform = 'Android'
                testSettings.testPlatform = 'playmode'
                testSettings.unityWarmup = true

                unityUnitTests(testSettings)
              }
            }
        }

        stage("Publish Nuget"){
          steps{
            nugetPublish([
                    solutionDir: "${env.CS_SOLUTION_DIRECTORY}",
                    releaseVersion: "${env.RELEASE_VERSION}"
            ])
          }
        }

        stage("Publish UPM"){
          when{
            expression { return env.UPM_PACKAGES != null }
          }
          steps{
            upmPublish([
                    upmPackages: "${env.UPM_PACKAGES}".split(","),
                    releaseVersion: "${env.RELEASE_VERSION}"
            ])
          }
        }

        stage('Tag') {
          when {
            expression { params.RELEASE }
          }
          steps{
            script{
              // This has to be in the scripts-scope because the git plugin defines another tag method, I think?
              tag([tagName: "client-${env.RELEASE_VERSION}"])
            }
          }
        }

        stage('Set Next Version') {
          when {
            expression { params.RELEASE }
          }
          steps{
            unsafeEcho("Running on: ${env.NODE_NAME}")

            setNextVersion([propertiesFile: "Jenkins.properties"])
          }
        }
      }
    }
  }

  post {
    always {
      addShortText(text: "${env.ENVIRONMENT}", background: 'black', border: 1, borderColor: 'fuchsia', color: 'white')

      notifySlackAboutBuildResult()
    }
  }
}
