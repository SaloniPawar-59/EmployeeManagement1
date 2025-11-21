pipeline {
    agent { label 'webapi-agent' }

    environment {
        AWS_REGION = 'us-east-1'
        EB_APP_NAME = 'EmployeeManagementApp'
        EB_ENV_NAME = 'EmployeeManagementApp-env'
        S3_BUCKET = 'my-eb-deployments-bucket'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/SaloniPawar-59/EmployeeManagement1.git'
            }
        }

        stage('Restore & Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish -c Release -o published'
                sh 'zip -r app.zip published/*'
            }
        }

        stage('Upload to S3') {
            steps {
                sh "aws s3 cp app.zip s3://${S3_BUCKET}/app.zip"
            }
        }

        stage('Deploy to Elastic Beanstalk') {
            steps {
                sh """
                aws elasticbeanstalk create-application-version \
                    --application-name ${EB_APP_NAME} \
                    --version-label v1-${BUILD_NUMBER} \
                    --source-bundle S3Bucket=${S3_BUCKET},S3Key=app.zip \
                    --region ${AWS_REGION}

                aws elasticbeanstalk update-environment \
                    --environment-name ${EB_ENV_NAME} \
                    --version-label v1-${BUILD_NUMBER} \
                    --region ${AWS_REGION}
                """
            }
        }
    }

    post {
        success {
            sh 'aws sns publish --topic-arn arn:aws:sns:us-east-1:123456789012:MySNSTopic --message "Deployment Successful!"'
        }
        failure {
            sh 'aws sns publish --topic-arn arn:aws:sns:us-east-1:123456789012:MySNSTopic --message "Deployment Failed!"'
        }
    }
}
