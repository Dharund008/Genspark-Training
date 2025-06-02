import torch
import json
import numpy as np
from sentence_transformers import SentenceTransformer
from sklearn.metrics.pairwise import cosine_similarity

class FAQService:
    def __init__(self, embeddings_path="faq_embeddings.pt", questions_path="questions.json", answers_path="answers.json"):
        self.device = "cuda" if torch.cuda.is_available() else "cpu"
        self.model = SentenceTransformer("sentence-transformers/all-mpnet-base-v2", device=self.device)
        self.embeddings = torch.load(embeddings_path, map_location=self.device)
        self.embeddings = self.embeddings.cpu().numpy() if self.device == "cpu" else self.embeddings.numpy()
        with open(questions_path, "r") as f:
            self.questions = json.load(f)
        with open(answers_path, "r") as f:
            self.answers = json.load(f)

    def get_answer(self, query, top_k=1):
        query_embedding = self.model.encode([query], convert_to_tensor=True)
        query_embedding = query_embedding.cpu().numpy()
        similarities = cosine_similarity(query_embedding, self.embeddings)[0]
        top_indices = similarities.argsort()[-top_k:][::-1]
        best_index = top_indices[0]
        best_score = similarities[best_index]
        # You can set a threshold for similarity if needed
        return self.answers[best_index], best_score

import sys

if __name__ == "__main__":
    faq_service = FAQService()
    if len(sys.argv) > 1:
        question = sys.argv[1]
        answer, score = faq_service.get_answer(question)
        print(answer)
    else:
        while True:
            user_input = input("Ask a question: ")
            answer, score = faq_service.get_answer(user_input)
            print(f"Answer: {answer} (Score: {score:.4f})")
