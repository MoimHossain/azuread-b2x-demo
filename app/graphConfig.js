// Add here the endpoints for MS Graph API services you would like to use.

const partnerProperty = 'extension_9472146313514090a46e5808f40ca0a2_PartnerName';

const graphConfig = {
    graphMeEndpoint: "https://graph.microsoft.com/v1.0/me?$select=Surname," + partnerProperty,
    graphMailEndpoint: "https://graph.microsoft.com/v1.0/me/messages"
};
