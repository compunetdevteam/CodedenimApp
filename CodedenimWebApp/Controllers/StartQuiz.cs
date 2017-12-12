using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CodedenimWebApp.Models;
using CodedenimWebApp.Service;
using CodedenimWebApp.ViewModels;
using CodeninModel.Quiz;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;


namespace CodedenimWebApp.Controllers
{
    public class StartQuiz
    {
        public ApplicationDbContext _db = new ApplicationDbContext();

        public async Task ProcessResult(int topicId, StudentTopicQuiz studentdetails, double scoreCount)
        {
           
            double sum = 1 * 2;
            double total = scoreCount * 1;
            var examLog = new QuizLog()
            {
                StudentId = studentdetails.StudentId,
                ModuleId = studentdetails.ModuleId,
                Score = total,
                TotalScore = sum,
                ExamTaken = true
            };
            _db.Set<QuizLog>().AddOrUpdate(examLog);
            await _db.SaveChangesAsync();

          

        }

        [ValidateInput(false)]
        public async Task SaveAnswer(DisplayQuestionViewModel model, string studentId, int questionId, string answer)
        {
            var question = await _db.StudentTopicQuizs.AsNoTracking().FirstOrDefaultAsync
            (s => s.StudentId.Equals(studentId) && s.ModuleId.Equals(model.ModuleId)
                  && s.QuestionNumber.Equals(questionId));
            if (question.Answer.ToUpper().Equals(answer.ToUpper()))
            {
                question.IsCorrect = true;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                question.FilledAnswer = answer;
                _db.Set<StudentTopicQuiz>().AddOrUpdate(question);
                // _db.Entry(question).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            else
            {
                question.IsCorrect = false;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                question.FilledAnswer = answer;
                _db.Set<StudentTopicQuiz>().AddOrUpdate(question);
                //_db.Entry(question).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public async Task SaveMultiChoiceAnswer(DisplayQuestionViewModel model, string checkedAnswer)
        {
            var question = _db.StudentTopicQuizs.AsNoTracking().FirstOrDefault(s => s.StudentId.Equals(model.StudentId)
                                                                                    && s.QuestionNumber.Equals(model.QuestionNo)
                                                                                    && s.ModuleId.Equals(model.ModuleId));
            string[] myAnswer = question.Answer.Split(',');
            StringBuilder answerbuilder = new StringBuilder();

            foreach (var item in myAnswer)
            {
                answerbuilder.Append(item);
            }

            string value = answerbuilder.ToString();
            string answer = SortStringAlphabetically(answerbuilder.ToString());


            if (answer.ToUpper().Equals(checkedAnswer.ToUpper()))
            {
                question.IsCorrect = true;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                _db.Entry(question).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            else
            {
                question.IsCorrect = false;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                _db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                await _db.SaveChangesAsync();
            }
        }

        public string SortStringAlphabetically(string str)
        {
            char[] foo = str.ToArray();
            Array.Sort(foo);
            return new string(foo);
        }

        public string CheckAnswerForSingleChoice(DisplayQuestionViewModel model)
        {
            if (model.SelectedAnswer.Equals(model.Option1))
            {
                return "A";
            }
            if (model.SelectedAnswer.Equals(model.Option2))
            {
                return "B";
            }
            if (model.SelectedAnswer.Equals(model.Option3))
            {
                return "C";
            }
            if (model.SelectedAnswer.Equals(model.Option4))
            {
                return "D";
            }
            return "";
        }

        public StudentTopicQuiz CheckQuestionType(DisplayQuestionViewModel model)
        {
            var questionType = _db.StudentTopicQuizs.FirstOrDefault(x => x.QuestionNumber.Equals(model.QuestionNo)
                                                                         && x.StudentId.Equals(model.StudentId) &&
                                                                         x.ModuleId.Equals(model.ModuleId));
            return questionType;
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}