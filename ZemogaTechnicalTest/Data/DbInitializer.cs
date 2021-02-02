using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ZemogaContext context)
        {
            context.Database.EnsureCreated();

            if (context.Roles.Any())
            {
                return;
            }

            var roles = new Role[]
            {
                new Role{RoleCode = "WR", RoleName = "Writer", RoleDesc="User able to create, edit and submit posts"},
                new Role{RoleCode = "ED", RoleName = "Editor", RoleDesc="User able to query for pending post and approve or reject their publishing"}
            };
            context.Roles.AddRange(roles);

            var status = new Status[]
            {
                new Status{StatusCode = "ED", StatusName="Edited", StatusDesc="The post was created or edited by their writer"},
                new Status{StatusCode = "PE", StatusName="Pending", StatusDesc="The post was submited by their writer"},
                new Status{StatusCode = "AP", StatusName="Approved", StatusDesc="The post was approved by a editor"},
                new Status{StatusCode = "RE", StatusName="Rejected", StatusDesc="The post was rejected by a editor"},
                new Status{StatusCode = "DE", StatusName="Deleted", StatusDesc="The post was deleted by a editor"}
            };
            context.Status.AddRange(status);
            context.SaveChanges();

            int writerRoleId = context.Roles.Where(u => u.RoleCode == "WR").FirstOrDefault().ID;
            int editorRoleId = context.Roles.Where(u => u.RoleCode == "ED").FirstOrDefault().ID;

            int edStatusId = context.Status.Where(u => u.StatusCode == "ED").FirstOrDefault().ID;
            int peStatusId = context.Status.Where(u => u.StatusCode == "PE").FirstOrDefault().ID;
            int apStatusId = context.Status.Where(u => u.StatusCode == "AP").FirstOrDefault().ID;
            int reStatusId = context.Status.Where(u => u.StatusCode == "RE").FirstOrDefault().ID;

            var users = new User[]
            {
                new User{CreatedDate = DateTime.Now, Password="123",RoleID=writerRoleId,UserFullname="Writer User",Username="writer"},
                new User{CreatedDate = DateTime.Now, Password="123",RoleID=editorRoleId,UserFullname="Editor User",Username="editor"}
            };
            context.Users.AddRange(users);

            context.SaveChanges();
            int userId = context.Users.Where(u => u.Username == "writer").FirstOrDefault().ID;
            var Posts = new Post[]
            {
                new Post
                {
                    AuthorID = userId,
                    StatusID = apStatusId,
                    PostName = "Smart Data Visualizations: Quality Assessment Algorithm",
                    PostContent = "The gap between a good and great data visualization is a vast chasm! The challenge is that we, and our HiPPOs, bring opinions and feelings and our perceptions of what will go viral to the conversation. This is entirely counter productive to distinguishing between bad, good, and great.",
                    PostComments = new List<PostComment>{
                        new PostComment{Comment="User's comment", UserID=userId},
                        new PostComment{Comment="Anonymous comment"}
                    }
                },
                
                new Post
                {
                    AuthorID = userId,
                    StatusID = apStatusId,
                    PostName = "Responses to Negative Data: Four Senior Leadership Archetypes.",
                    PostContent = "Not everything a company does works out. (That is different from everything that a company is doing not working out. If you are in the data business – my bread, butter and tofu – you often carry the burden of being the bearer of bad news. A decade ago, data people delivered a lot less bad news because so little could be measured with any degree of confidence."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = apStatusId,
                    PostName = "The Impact Matrix | A Digital Analytics Strategic Framework",
                    PostContent = "The universe of digital analytics is massive and can seem as complex as the cosmic universe. With such big, complicated subjects, we can get lost in the vast wilderness or become trapped in a silo. We can wander aimlessly, or feel a false sense of either accomplishment or frustration. Consequently, we lose sight of where we are, how we are doing and which direction is true north."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = peStatusId,
                    PostName = "Deliver Step Change Impact: Marketing & Analytics Obsessions",
                    PostContent = "Some moments in time are perfect to reflect on where you are, what your priorities are, and then consider what you should start-stop-continue. In those moments, you are not thinking of delivering incremental change… You are driven by a desire to deliver a step change (a large or sudden discontinuous change, especially one that makes things better – I’m borrowing the concept from mathematics and technology, from “step function”)."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = peStatusId,
                    PostName = "Six Nudges: Creating A Sense Of Urgency For Higher Conversion Rates!",
                    PostContent = "By every indicator available, ecommerce is continuing to grow at an insane speed. Although it may seem impossible to imagine with ecommerce already totaling up to 5% of overall commerce, there’s astronomical growth still to come. Still, I’m heartbroken that some the simplest elements of ecommerce stink so much."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = peStatusId,
                    PostName = "Closing Data's Last-Mile Gap: Visualizing For Impact!",
                    PostContent = "I worry about data’s last-mile gap a lot. As a lover of data-influenced decision making, perhaps you worry as well. A lot of hard work has gone into collecting the requirements and implementation. An additional massive investment was made in the effort to perform ninja like analysis. The end result was a collection trends and insights."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = reStatusId,
                    PostName = "Five Strategies for Slaying the Data Puking Dragon.",
                    PostContent = "If you bring sharp focus, you increase chances of attention being diverted to the right places. That in turn will drive smarter questions, which will elicit thoughtful answers from available data. The result will be data-influenced actions that result in a long-term strategic advantage. It all starts with sharp focus."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = reStatusId,
                    PostName = "Digital Analytics + Marketing Career Advice: Your Now, Next, Long Plan",
                    PostContent = "The rapid pace of innovation and the constantly exploding collection of possibilities is a major contributor to the fun we all have in digital jobs. There is never a boring moment, there is never time when you can’t do something faster or smarter."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = apStatusId,
                    PostName = "The Artificial Intelligence Opportunity: A Camel to Cars Moment",
                    PostContent = "Over the last couple years, I’ve spent an increasing amount of time diving into the possibilities Deep Learning (DL) offers in terms of what we can do with Artificial Intelligence (AI). Some of these possibilities have already been realized (more on this later in the post). And, I could not be more excited to see them out in the world."
                },
                new Post
                {
                    AuthorID = userId,
                    StatusID = apStatusId,
                    PostName = "Artificial Intelligence: Implications On Marketing, Analytics, And You",
                    PostContent = "A rare post today. It looks a little further out into the future than I normally tend to. It attempts to simplify a topic that has more than it’s share of coolness, confusion and complexity. While the phrase Artificial Intelligence has been around since the first human wondered if she could go further if she had access to entities with inorganic intelligence, it truly jumped the shark shifted into high gear in 2016. "
                },
            };
            context.Posts.AddRange(Posts);

            context.SaveChanges();
        }
    }
}
