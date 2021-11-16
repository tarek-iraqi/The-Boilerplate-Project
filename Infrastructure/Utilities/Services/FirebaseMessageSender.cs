using Application.DTOs;
using Application.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utilities
{
    public class FirebaseMessageSender : IFirebaseMessageSender
    {
        private readonly FirebaseApp app;
        public FirebaseMessageSender(IApplicationConfiguration config)
        {
            try
            {
                var firebaseConfiguration = config.GetFirebaseSettings();
                var privateKeyValue = firebaseConfiguration.private_key.Replace("\\n", "\n");
                firebaseConfiguration.private_key = privateKeyValue;

                var jsonFormat = JsonSerializer.Serialize(firebaseConfiguration);

                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonFormat)
                }, KeyValueConstants.FirebaseApp);
            }
            catch
            {
                app = FirebaseApp.GetInstance(KeyValueConstants.FirebaseApp);
            }
        }

        public async Task<FirebaseSendMessageResultDTO> SendToTopic(string title, string body,
            string topicName, Dictionary<string, string> data = null, bool isTest = false)
        {
            var fcm = FirebaseMessaging.GetMessaging(app);

            Message message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Topic = topicName
            };

            try
            {
                var result = await fcm.SendAsync(message, isTest);

                return new FirebaseSendMessageResultDTO
                {
                    is_success = !string.IsNullOrWhiteSpace(result),
                    message_id = result
                };
            }
            catch (Exception ex)
            {
                return new FirebaseSendMessageResultDTO
                {
                    is_success = false,
                    error = ex.Message
                };
            }
        }

        public async Task<FirebaseSendMessageResultDTO> SendToDevice(string title, string body,
            string deviceToken, Dictionary<string, string> data = null, bool isTest = false)
        {
            var fcm = FirebaseMessaging.GetMessaging(app);

            Message message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Token = deviceToken
            };

            try
            {
                var result = await fcm.SendAsync(message, isTest);

                return new FirebaseSendMessageResultDTO
                {
                    is_success = !string.IsNullOrWhiteSpace(result),
                    message_id = result
                };
            }
            catch (Exception ex)
            {
                return new FirebaseSendMessageResultDTO
                {
                    is_success = false,
                    error = ex.Message
                };
            }
        }

        public async Task<FirebaseMultiDevicesSendMessageResultDTO> SendToDevices(string title, string body,
            List<string> devicesTokens, Dictionary<string, string> data = null, bool isTest = false)
        {
            var fcm = FirebaseMessaging.GetMessaging(app);

            MulticastMessage message = new MulticastMessage()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Tokens = devicesTokens
            };

            var result = await fcm.SendMulticastAsync(message, isTest);

            return new FirebaseMultiDevicesSendMessageResultDTO
            {
                failure_count = result.FailureCount,
                success_count = result.SuccessCount,
                send_result = result.Responses.Select(response => new FirebaseSendMessageResultDTO
                {
                    is_success = response.IsSuccess,
                    message_id = response.MessageId,
                    error = response.Exception.Message
                })
            };
        }
    }
}
