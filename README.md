# Abtract 


 Writing software requirements in natural language (NL) makes the requirement incon-sistent, ambiguous and open to several conflicting interpretations. Thus, NL is prone to many errors because of its ambiguous nature. Errors that often occurs in requirements process propagate to all subsequent processes. Identifying and correcting errors during the requirements process is less ex-pensive than fixing them after implementation. NLP techniques can be used to perform sentiment analysis on requirements text to identify expressions of doubt, uncertainty, or disagreement. This has the potential to help in identifying potential inconsistencies or conflicts in the requirements.  Conversational bots (chatbots) can be an effective way to gather requirements from a large number of users, as they can handle multiple concurrent conversations and provide fast and convenient access to stored information and NLP services. This paper proposes a technical solution for using Chatbots along with NLP for eliciting requirements, classifying them, and ensuring the quality of the elicited requirements.



# Main Components	
1.	Dialog User Interface: a provisioned bot that can run as a service account in chat applications like Facebook messenger.
2.	Bot Web Service: C# .NET Core Application that uses Microsoft Bot framework SDK, an open-source framework for building conversational bots flows using ASP.NET
3.	OpenAI API:  GPT-3 (Generative Pre-trained Transformer 3) is a state-of-the-art language processing AI model developed by OpenAI. It has 175 bil-lion parameters, making it one of the largest models of its kind. GPT-3 is trained on a diverse range of internet text, allowing it to generate human-like text, answer ques-tions, and complete a variety of language tasks. It can be fine-tuned for specific ap-plications such as language translation, summarization, and content creation.
4.	Elicited Requirements Register: SQL Database for storing previously elicited requirements.
"# TheRequirementsBot" 
