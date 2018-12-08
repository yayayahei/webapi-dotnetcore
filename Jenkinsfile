pipeline {
  agent any
  stages {
    stage('build') {
      steps {
        bat './build.bat'
      }
    }
    stage('deploy') {
      steps {
        powershell(script: './deploy.ps1', encoding: 'utf-8')
      }
    }
  }
}